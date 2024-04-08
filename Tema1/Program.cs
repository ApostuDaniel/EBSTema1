using System.Collections.Concurrent;
using System.Diagnostics;
using Tema1;

int numPublications = 1000000;
int numSubscriptions = 1000000;
int threadCount = 2;

Stopwatch stopwatch = new();

stopwatch.Start();
List<Publication> publications = GenerateRandomPublications(numPublications, threadCount);
stopwatch.Stop();

Console.WriteLine($"{numPublications} publications generated in {stopwatch.ElapsedMilliseconds} ms with {threadCount} threads");

List<SubscriptionWeight> subWeights = new() {
    new SubscriptionWeight() { Attribute = Field.COMPANY, Weight = 0.9, EqWeight = 0.7 },
    new SubscriptionWeight() { Attribute = Field.VALUE, Weight = 0.2, EqWeight = 0.5 },
    new SubscriptionWeight() { Attribute = Field.DROP, Weight = 0.1, EqWeight = 0.0 },
};

stopwatch.Restart();
List<Subscription> subscriptions = GenerateRandomSubscriptions(numSubscriptions, subWeights, threadCount);
stopwatch.Stop();

Console.WriteLine($"{subscriptions.Count} subscriptions generated in {stopwatch.ElapsedMilliseconds} ms with {threadCount} threads");

Console.WriteLine("--------Publications---------------");
//publications.ToList().ForEach(x => Console.WriteLine(x));



static List<Publication> GenerateRandomPublications(int numPublications, int numThreads)
{
    ConcurrentBag<Publication> publications = new();
    Parallel.For(0, numPublications, new ParallelOptions { MaxDegreeOfParallelism = numThreads }, _ => publications.Add(Publication.CreateRandPublication()));

    return publications.ToList();
}

static List<Subscription> GenerateRandomSubscriptions(int numSubs, List<SubscriptionWeight> subWeights, int threads)
{
    if (subWeights == null || subWeights.Count == 0)
    {
        throw new Exception("You need to provide weights for subscription fields");
    }
    var weightSum = subWeights.Sum(x => x.Weight);

    if (weightSum < 1.0)
    {
        throw new Exception("The weights of all fields must sum at least 100%");
    }

    List<int> countByAttribute = subWeights.ConvertAll(x => Convert.ToInt32(x.Weight * numSubs));
    List<int> countByAttributeWithEq = new();
    for (int i = 0; i < countByAttribute.Count; i++)
    {
        countByAttributeWithEq.Add(Convert.ToInt32(subWeights[i].EqWeight * countByAttribute[i]));
    }

    var numSingleAttributeSubscriptions = countByAttribute.Sum();
    ConcurrentDictionary<int, Subscription> singleAttributeSubscriptions = new();
    Parallel.For(0, numSingleAttributeSubscriptions, new ParallelOptions { MaxDegreeOfParallelism = threads },
        idx =>
            {
                int lastStep = 0;
                for (int i = 0; i < subWeights.Count; i++)
                {
                    if (idx >= lastStep && idx < lastStep + countByAttributeWithEq[i])
                    {
                        singleAttributeSubscriptions.TryAdd(idx, RandSingleFieldSubscription(subWeights[i].Attribute, true));
                        break;
                    }
                    else if (idx >= lastStep && idx < lastStep + countByAttribute[i])
                    {
                        singleAttributeSubscriptions.TryAdd(idx, RandSingleFieldSubscription(subWeights[i].Attribute, false));
                        break;
                    }
                    lastStep += countByAttribute[i];
                }
            });
    // TO DO: make sure that the subscriptions are in order of the weights provided
    List<Subscription> oneFieldSubscriptions = new();
    for(int i = 0; i < numSingleAttributeSubscriptions; i++)
    {
        oneFieldSubscriptions.Add(singleAttributeSubscriptions[i]);
    }

    if (weightSum <= 1.000001)
    {
        return oneFieldSubscriptions;
    }

    List<ConcurrentBag<Subscription>> subscriptionBags = new();
    List<List<int>> availableCombinations = new();
    List<List<int>> componentFields = new();
    Dictionary<(int i, int j), int> resultCombinations = new();

    var start = 0;
    for(int i = 0;i < subWeights.Count;i++)
    {
        var subBag = new ConcurrentBag<Subscription>(oneFieldSubscriptions.GetRange(start, countByAttribute[i]));
        subscriptionBags.Add(subBag);
        var subBagAvailableCombinations = new List<int>(Enumerable.Range(0, subWeights.Count));
        subBagAvailableCombinations.RemoveAt(i);
        availableCombinations.Add(subBagAvailableCombinations);
        componentFields.Add(new List<int>() { i });
        resultCombinations.Add((i, i), i);
        start += countByAttribute[i];
    }

    for(int i = 0; i < subscriptionBags.Count; i++)
    {
        for(int j = i + 1;  j < subscriptionBags.Count; j++)
        {
            if (!componentFields[i].Intersect(componentFields[j]).Any())
            {
                if (resultCombinations.ContainsKey((i, j)))
                {
                    continue;
                }
                var currentComponentFields = componentFields[i].Union(componentFields[j]).ToList();
                var bagIndex = subscriptionBags.Count;
                for(int k = j; k < componentFields.Count; k++)
                {
                    if (componentFields[k].TrueForAll(currentComponentFields.Contains) && componentFields[k].Count == currentComponentFields.Count)
                    {
                        bagIndex = k;
                        break;
                    }
                }
                resultCombinations.Add((i, j), bagIndex);
                resultCombinations.Add((j, i), bagIndex);
                // Meaning we should add a new bag
                if (bagIndex == subscriptionBags.Count)
                {
                    subscriptionBags.Add(new ConcurrentBag<Subscription>());
                    availableCombinations.Add(availableCombinations[i].Intersect(availableCombinations[j]).ToList());
                    componentFields.Add(currentComponentFields);
                    //Reset i if new bag added
                    i = 0;
                }
            }
        }
    }

    var bagCount = subscriptionBags.Count;

    Parallel.For(0, oneFieldSubscriptions.Count - numSubs, new ParallelOptions() { MaxDegreeOfParallelism = threads },
        idx =>
        {
            bool combinationsSuccesfull = false;
            while (!combinationsSuccesfull)
            {
                var sourceBagId = Random.Shared.Next(bagCount);
                var sourceBag = subscriptionBags[sourceBagId];
                if (availableCombinations[sourceBagId]?.Any() != true
                || !sourceBag.Any())
                {
                    continue;
                }
                var randBagIdIndex = Random.Shared.Next(availableCombinations[sourceBagId].Count);
                var partnerBagId = availableCombinations[sourceBagId][randBagIdIndex];
                var partnerBag = subscriptionBags[partnerBagId];

                if (!partnerBag.Any())
                {
                    continue;
                }

                Subscription sourceSub;
                Subscription partnerSub;

                bool sourceExtractSuccess = sourceBag.TryTake(out sourceSub);
                if (!sourceExtractSuccess)
                {
                    continue;
                }
                bool partnerExtractSuccess = partnerBag.TryTake(out partnerSub);
                if (!partnerExtractSuccess)
                {
                    sourceBag.Add(sourceSub);
                    continue;
                }

                var combinedSubscription = Subscription.CombineSubscriptions(sourceSub, partnerSub);
                var resultBagId = resultCombinations[(sourceBagId, partnerBagId)];
                subscriptionBags[resultBagId].Add(combinedSubscription);
                combinationsSuccesfull = true;
            }
        });

    List<Subscription> result = new List<Subscription>();
    foreach(var subBag in subscriptionBags)
    {
        result.AddRange(subBag);
    }

    return result;
}

static Subscription RandSingleFieldSubscription(Field attribute, bool useEq)
{
    var op = Operator.EQ;
    if (!useEq)
    {
        op = PickRandNonEqOperator();
    }

    ISubscriptionField subscriptionField = attribute switch
    {
        Field.COMPANY => new SubscriptionField<string>()
        {
            Attribute = attribute.ToString(),
            Operator = op,
            Value = FieldRestrictions.CompayNames[Random.Shared.Next(FieldRestrictions.CompayNames.Count)]
        },
        Field.VALUE => new SubscriptionField<double>()
        {
            Attribute = attribute.ToString(),
            Operator = op,
            Value = FieldRestrictions.GetRandomNumber(FieldRestrictions.ValueRange)
        },
        Field.VARIATION => new SubscriptionField<double>()
        {
            Attribute = attribute.ToString(),
            Operator = op,
            Value = FieldRestrictions.GetRandomNumber(FieldRestrictions.VariationRange)
        },
        Field.DROP => new SubscriptionField<double>()
        {
            Attribute = attribute.ToString(),
            Operator = op,
            Value = FieldRestrictions.GetRandomNumber(FieldRestrictions.DropRange)
        },
        Field.DATE => new SubscriptionField<DateOnly>()
        {
            Attribute = attribute.ToString(),
            Operator = op,
            Value = FieldRestrictions.DateRange.startDate.AddDays(Random.Shared.Next(FieldRestrictions.DateRange.maxDaysAdd))
        },
    };

    return new Subscription(new List<ISubscriptionField> { subscriptionField });
}

static Operator PickRandNonEqOperator()
{
    switch (Random.Shared.Next(5))
    {
        case 0:
            return Operator.GT;
        case 1:
            return Operator.LT;
        case 2:
            return Operator.LE;
        case 3:
            return Operator.NEQ;
        case 4:
            return Operator.GE;
        default: throw new NotImplementedException();
    }
}