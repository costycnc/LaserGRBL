using System;
using System.Collections.Generic;

namespace CsPotrace
{
    public static class CostyCNCOptimizer
    {
        // Algoritmo geometrico di riordinamento continuo in coordinate pixel - CostyCNC Edition
        public static List<Path> OptimizePixelPaths(List<Path> costyx)
        {
            if (costyx == null || costyx.Count == 0) return costyx;

            // Questa lista conterrà i cammini riordinati e ottimizzati
            List<Path> pathx = new List<Path>();

            // Punto di ancoraggio iniziale (coordinate pixel 0,0) per agganciare il primo profilo
            dPoint currentAnchor = new dPoint(0, 0);

            while (costyx.Count > 0)
            {
                double minDiff = double.MaxValue;
                int targetPathIndex = 0;
                int targetPointIndex = 0;

                // Ciclo m: Scansiona tutti i cammini (poligoni di pixel) rimasti disordinati
                for (int m = 0; m < costyx.Count; m++)
                {
                    dPoint[] points = costyx[m].pt; 
                    if (points == null || points.Length == 0) continue;

                    // Ciclo n: Scansiona i singoli nodi interni (salto di 10 punti per fluidità)
                    for (int n = 0; n < points.Length; n += 10)
                    {
                        double x2 = currentAnchor.x - points[n].x;
                        double y2 = currentAnchor.y - points[n].y;

                        // Calcolo della distanza euclidea quadratica: la tua intuizione originale!
                        double currDiff = (x2 * x2) + (y2 * y2);

                        if (currDiff < minDiff)
                        {
                            minDiff = currDiff;
                            targetPathIndex = m;   
                            targetPointIndex = n;  
                        }
                    }
                }

                // --- FASE DI ESTRAZIONE E ROTAZIONE GEOMETRICA (Splice & Rotate) ---
                Path selectedPath = costyx[targetPathIndex];
                costyx.RemoveAt(targetPathIndex);

                dPoint[] originalPoints = selectedPath.pt;
                List<dPoint> rotatedPoints = new List<dPoint>();

                // Ruota il vettore dei punti per allineare l'attacco al punto geometricamente più vicino
                for (int k = targetPointIndex; k < originalPoints.Length; k++) rotatedPoints.Add(originalPoints[k]);
                for (int k = 0; k < targetPointIndex; k++) rotatedPoints.Add(originalPoints[k]);

                // Chiude il tracciato ad anello (come il tuo p.push(p))
                if (rotatedPoints.Count > 0)
                {
                    rotatedPoints.Add(rotatedPoints);
                }

                selectedPath.pt = rotatedPoints.ToArray();
                pathx.Add(selectedPath);

                // Il nuovo punto di ancoraggio diventa l'ultimo punto del tracciato inserito
                currentAnchor = rotatedPoints[rotatedPoints.Count - 1];
            }

            return pathx;
        }
    }
}
