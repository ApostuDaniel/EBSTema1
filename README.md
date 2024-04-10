# Tema1
## Tip paralelizare: threads
## Factor paralelizare: 8
Fara paralelizare:  
1000000 publications generated in 344 ms with 1 threads
1000000 subscriptions generated in 3360 ms with 1 threads

Cu paralelizare:  
 
1000000 publications generated in 259 ms with 4 threads  
1000000 subscriptions generated in 2620 ms with 4 threads  
  
1000000 publications generated in 279 ms with 8 threads
1000000 subscriptions generated in 2105 ms with 8 threads  
  
1000000 publications generated in 597 ms with 16 threads  
1000000 subscriptions generated in 2414 ms with 16 threads  

Beneficiile paralelizarii se vad deabia de la 100000 de publicatii/subscriptii generate.  

Procesor AMD Ryzen 7 4800HS, base speed 2.90GHz, 8 cores, 16 logical procesors.  
