# Matrix Calculator - Porównanie Wydajności Mnożenia Macierzy

## Opis Programu

Program **MatrixCalculator** to narzędzie badawcze do porównania wydajności mnożenia macierzy za pomocą trzech różnych podejść:

1. **Obliczenia sekwencyjne** - tradycyjne mnożenie macierzy w jednym wątku
2. **Parallel (TPL)** - użycie biblioteki Task Parallel Library z `Parallel.For()`
3. **Threads** - ręczne zarządzanie wątkami przy użyciu klasy `Thread`

Program generuje dwie losowe macierze o rozmiarze 500×500 i mierzy średni czas wykonania operacji mnożenia macierzy dla każdej z trzech metod z uwzględnieniem różnej liczby wątków (2, 4, 8, 16).

## Parametry Badań

- **Rozmiar macierzy:** 500 × 500
- **Liczba prób:** 10 (obliczana jest średnia)
- **Liczby wątków testowane:** 2, 4, 8, 16
- **Konfiguracja:** Release (zoptymalizowana)

## Wyniki Badań

### Konfiguracja: macierze 500×500, średnia z 10 prób

```
--- OBLICZENIA SEKWENCYJNE ---
Średni czas: 283,66 ms

--- OBLICZENIA PARALLEL (Zadanie 1 - TPL) ---
Wątki: 2  | Średni czas: 159,43 ms
Wątki: 4  | Średni czas: 105,79 ms
Wątki: 8  | Średni czas: 88,53 ms
Wątki: 16 | Średni czas: 84,07 ms

--- OBLICZENIA THREAD (Zadanie 2 - Manual Threading) ---
Wątki: 2  | Średni czas: 184,39 ms
Wątki: 4  | Średni czas: 126,18 ms
Wątki: 8  | Średni czas: 86,10 ms
Wątki: 16 | Średni czas: 91,03 ms
```

## Analiza i Porównanie

### 1. Przyspieszenie (Speedup)

#### Metoda Parallel (TPL)
| Wątki | Czas [ms] | Przyspieszenie | Efektywność |
|-------|-----------|----------------|-------------|
| 1 (sekwencyjnie) | 283,66 | 1,00x | 100% |
| 2 | 159,43 | 1,78x | 89% |
| 4 | 105,79 | 2,68x | 67% |
| 8 | 88,53 | 3,21x | 40% |
| 16 | 84,07 | 3,38x | 21% |

#### Metoda Threads (ręczne)
| Wątki | Czas [ms] | Przyspieszenie | Efektywność |
|-------|-----------|----------------|-------------|
| 1 (sekwencyjnie) | 283,66 | 1,00x | 100% |
| 2 | 184,39 | 1,54x | 77% |
| 4 | 126,18 | 2,25x | 56% |
| 8 | 86,10 | 3,29x | 41% |
| 16 | 91,03 | 3,12x | 20% |

### 2. Wnioski z Badań

#### **Porównanie Metod**

1. **Parallel (TPL) jest znacznie szybsza** niż manual threading:
   - 2 wątki: **13,9% szybsza** (159,43 ms vs 184,39 ms)
   - 4 wątki: **16,1% szybsza** (105,79 ms vs 126,18 ms)
   - 8 wątków: **2,8% szybsza** (88,53 ms vs 86,10 ms) - tu już marginal
   - 16 wątków: **7,6% wolniejsza** (84,07 ms vs 91,03 ms) - efekt kontekstowego przełączania

2. **Dlaczego Parallel jest lepsze?**
   - TPL automatycznie optymalizuje rozkład pracy
   - Lepsze zarządzanie scheduler'em
   - Mniejszy overhead synchronizacji
   - Lepsze wykorzystanie cache CPU

3. **Manual Threading**
   - Pokazuje narzęcie do zrozumienia podstaw paralelizacji
   - Mniej elastyczne
   - Wyższy overhead przy większej liczbie wątków
   - Trudniejsze w debugowaniu

## Techniczne Detale

### Metody Implementacji

**1. MultiplySequential** - O(n³) w jednym wątku
```
Pętla po wierszach A
  Pętla po kolumnach B
    Pętla po kolumnach A (sumowanie iloczynu)
```

**2. MultiplyParallel** - Parallel.For z MaxDegreeOfParallelism
- Rozdziela wiersze macierzy A między wątkami
- TPL samodzielnie zarządza wątkami i scheduler'em
- Gwarantuje poprawną synchronizację dostępu do wyniku

**3. MultiplyThreads** - Ręczne tworzenie wątków
- Każdy wątek odpowiada za zestaw wierszy
- Użycie `Thread.Join()` do czekania na zakończenie
- Wymaga ręcznego zarządzania synchronizacją

## Kompilacja i Uruchomienie

### Wymagania
- .NET 8.0 SDK

### Kompilacja
```bash
dotnet build -c Release
```

### Uruchomienie
```bash
cd dotNetlab3
dotnet run --configuration Release
```

### Wyniki wypisze się na konsolę z czasami dla każdej metody.

## Modyfikacja Parametrów

W pliku `Program.cs` można zmienić:
- `size` - rozmiar macierzy (domyślnie: 500)
- `trials` - liczba prób (domyślnie: 10)
- `threadCounts` - liczby wątków do testowania (domyślnie: 2, 4, 8, 16)
- `showMatrix` - wyświetlanie pełnych macierzy (domyślnie: false)


