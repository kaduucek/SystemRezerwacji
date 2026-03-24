# System rezerwacji w przychodni lekarskiej

## Opis Projektu

Projekt zakłada stworzenie desktopowej aplikacji okienkowej (WPF), która ma na celu cyfryzację procesów w przychodni lekarskiej. System umożliwia rejestrację pacjentów, zarządzanie wizytami, dostęp do historii leczenia oraz podstawowe funkcje administracyjne. Aplikacja opiera się na bazie danych SQL Server, co zapewnia integralność i bezpieczeństwo przechowywanych danych medycznych.

Aplikacja służy do kompleksowego zarządzania obiegiem informacji w przychodni. Umożliwia:

- **Umówienie wizyty** wraz z wyborem daty, godziny oraz lekarza
- **Anulowanie wizyty**
- **Zmianę** daty, godziny wizyty lub lekarza
- **Sprawdzenie historii** wizyt
- **Sprawdzenie wolnych terminów** wizyt

Aplikacja przeznaczona jest dla **pacjentów przychodni**.

---

## Wymagania funkcjonalne

| Lp. | Wymaganie | Opis |
|-----|-----------|------|
| 1. | **Rejestracja i logowanie użytkownika** | Użytkownik (pacjent) może założyć konto w systemie oraz zalogować się, aby uzyskać dostęp do swoich danych. |
| 2. | **Przeglądanie wolnych terminów** | System umożliwia wyświetlenie dostępnych terminów wizyt u wybranego lekarza w określonym przedziale czasowym. |
| 3. | **Umawianie wizyty** | Pacjent może wybrać lekarza, datę i godzinę, a następnie potwierdzić rezerwację wizyty. |
| 4. | **Anulowanie wizyty** | Użytkownik może anulować zaplanowaną wizytę (np. na co najmniej 24 godziny przed jej rozpoczęciem). |
| 5. | **Modyfikacja wizyty** | Istnieje możliwość zmiany terminu lub lekarza dla już umówionej wizyty. |
| 6. | **Przeglądanie historii wizyt** | System wyświetla listę przeszłych i przyszłych wizyt pacjenta wraz ze szczegółami (data, lekarz, status). |

---

## Wymagania niefunkcjonalne

| Lp. | Wymaganie | Opis |
|-----|-----------|------|
| 1. | **Bezpieczeństwo danych** | Dostęp do systemu wyłącznie po uwierzytelnieniu; dane medyczne przechowywane zgodnie z RODO; hasła przechowywane w formie zahashowanej. |
| 2. | **Integralność danych** | Wykorzystanie transakcji w bazie danych SQL Server, aby zapobiec konfliktom przy równoczesnym umawianiu wizyt przez wielu użytkowników. |
| 3. | **Wydajność** | Czasy odpowiedzi aplikacji nie przekraczają 2 sekund dla standardowych operacji (np. wyświetlenie listy wolnych terminów). |
| 4. | **Dostępność** | Aplikacja powinna działać w godzinach pracy przychodni (7:00–20:00) z planowaną dostępnością na poziomie 99,5%. |
| 5. | **Użyteczność** | Interfejs użytkownika oparty na wzorcach WPF (MVVM) zapewnia intuicyjną nawigację oraz czytelny układ elementów (kalendarz, listy wizyt). |
| 6. | **Skalowalność** | Architektura warstwowa umożliwia ewentualne rozbudowanie systemu o moduł dla recepcji lub panel lekarza bez konieczności przebudowy całości. |

---

## Przypadki użycia

### UC-01: Umówienie wizyty

| Element | Opis |
|---------|------|
| **Aktor** | Pacjent |
| **Warunek początkowy** | Pacjent jest zalogowany. |
| **Scenariusz główny** | 1. Pacjent wybiera opcję „Nowa wizyta”.<br>2. System wyświetla listę lekarzy wraz ze specjalizacjami.<br>3. Pacjent wybiera lekarza.<br>4. System pokazuje dostępne terminy w formie kalendarza.<br>5. Pacjent wybiera dogodną datę i godzinę.<br>6. System wyświetla podsumowanie (lekarz, data, godzina).<br>7. Pacjent potwierdza rezerwację.<br>8. System zapisuje wizytę w bazie danych i wysyła potwierdzenie e-mail. |
| **Warunek końcowy** | Wizyta zostaje umówiona i widnieje na liście wizyt pacjenta. |

---

### UC-02: Anulowanie wizyty

| Element | Opis |
|---------|------|
| **Aktor** | Pacjent |
| **Warunek początkowy** | Pacjent jest zalogowany i posiada co najmniej jedną przyszłą wizytę. |
| **Scenariusz główny** | 1. Pacjent przechodzi do zakładki „Moje wizyty”.<br>2. System wyświetla listę nadchodzących wizyt.<br>3. Pacjent wybiera wizytę do anulowania i klika „Anuluj”.<br>4. System prosi o potwierdzenie operacji.<br>5. Pacjent potwierdza anulowanie.<br>6. System usuwa wizytę (lub zmienia jej status na „anulowana”) i zwalnia termin w kalendarzu lekarza. |
| **Warunek końcowy** | Wizyta zostaje anulowana, a zwolniony termin staje się ponownie dostępny. |

---

### UC-03: Zmiana terminu wizyty

| Element | Opis |
|---------|------|
| **Aktor** | Pacjent |
| **Warunek początkowy** | Pacjent jest zalogowany i posiada co najmniej jedną przyszłą wizytę. |
| **Scenariusz główny** | 1. Pacjent wybiera wizytę z listy i opcję „Zmień termin”.<br>2. System wyświetla dostępne terminy u tego samego lekarza (lub umożliwia zmianę lekarza).<br>3. Pacjent wybiera nowy termin.<br>4. System aktualizuje dane wizyty.<br>5. System wysyła potwierdzenie zmiany na e-mail pacjenta. |
| **Warunek końcowy** | Wizyta zostaje przeniesiona na nowy termin, stary termin zostaje zwolniony. |

---

## Model danych

### Główne encje

#### Pacjent
| Pole | Opis |
|------|------|
| `IdPacjenta` (PK) | Unikalny identyfikator pacjenta |
| `Imię` | Imię pacjenta |
| `Nazwisko` | Nazwisko pacjenta |
| `PESEL` | Numer PESEL |
| `Email` | Adres e-mail |
| `HaszHasła` | Zahaszowane hasło |
| `NumerTelefonu` | Numer kontaktowy |
| `DataRejestracji` | Data utworzenia konta |

#### Lekarz
| Pole | Opis |
|------|------|
| `IdLekarza` (PK) | Unikalny identyfikator lekarza |
| `Imię` | Imię lekarza |
| `Nazwisko` | Nazwisko lekarza |
| `Specjalizacja` | Specjalizacja lekarska |
| `Email` | Adres e-mail |
| `NumerTelefonu` | Numer kontaktowy |

#### Wizyta
| Pole | Opis |
|------|------|
| `IdWizyty` (PK) | Unikalny identyfikator wizyty |
| `IdPacjenta` (FK) | Odniesienie do pacjenta |
| `IdLekarza` (FK) | Odniesienie do lekarza |
| `DataGodzinaRozpoczecia` | Data i godzina rozpoczęcia wizyty |
| `CzasTrwania` | Czas trwania w minutach |
| `Status` | Status: zaplanowana, odwołana, zrealizowana |

#### Dostępność (opcjonalnie)
| Pole | Opis |
|------|------|
| `IdDostępności` (PK) | Unikalny identyfikator |
| `IdLekarza` (FK) | Odniesienie do lekarza |
| `DzieńTygodnia` | Dzień tygodnia |
| `GodzinaRozpoczęcia` | Godzina rozpoczęcia pracy |
| `GodzinaZakończenia` | Godzina zakończenia pracy |
| `OkresWażnościOd` | Początek obowiązywania grafiku |
| `OkresWażnościDo` | Koniec obowiązywania grafiku |

> *Uwaga: Tabela dostępności może być zastąpiona generowaniem terminów na podstawie grafiku.*

#### HistoriaMedyczna
| Pole | Opis |
|------|------|
| `IdWpisu` (PK) | Unikalny identyfikator wpisu |
| `IdPacjenta` (FK) | Odniesienie do pacjenta |
| `DataWpisu` | Data dodania wpisu |
| `Opis` | Treść wpisu medycznego |
| `IdWizyty` (FK) | Opcjonalne odniesienie do wizyty |

### Relacje między encjami

- **Pacjent** – **Wizyta**: `1 : N`
- **Lekarz** – **Wizyta**: `1 : N`
- **Lekarz** – **Dostępność**: `1 : N`
- **Pacjent** – **HistoriaMedyczna**: `1 : N`

---

## Architektura systemu

System został zaprojektowany w architekturze warstwowej z wyraźnym podziałem odpowiedzialności:
###  Warstwa prezentacji
**Aplikacja WPF (MVVM)**
- Widoki (XAML)
- ViewModele
- Walidacja danych wejściowych

###  Warstwa logiki biznesowej
**Biblioteka klas .NET**
- Reguły biznesowe
- Walidacja terminów
- Zarządzanie stanem

###  Warstwa dostępu do danych (DAL)
**Entity Framework Core**
- Repozytoria
- Unit of Work
- Migracje

###  Baza danych
**Microsoft SQL Server**
- Tabele
- Klucze główne/obce
- Indeksy
- Transakcje

### Przepływ danych
[UI: WPF/MVVM] ⇄ [ViewModel] ⇄ [Warstwa Biznesowa] ⇄ [DAL (Entity Framework)] ⇄ [SQL Server]


---

## Technologie i uzasadnienie

| Technologia | Uzasadnienie |
|-------------|--------------|
| **.NET 8 / .NET 6** | Stabilna, nowoczesna platforma do tworzenia aplikacji desktopowych i backendu. Długoterminowe wsparcie (LTS). |
| **WPF** | Sprawdzona technologia do tworzenia bogatych aplikacji okienkowych na Windows z wykorzystaniem XAML i wzorca MVVM. |
| **Entity Framework Core** | Upraszcza dostęp do bazy danych, zapewnia bezpieczeństwo typów, obsługę migracji oraz transakcji. |
| **Microsoft SQL Server** | Profesjonalny system zarządzania bazą danych spełniający wymogi bezpieczeństwa, integralności i wydajności dla systemów medycznych. |
| **MVVM Toolkit** | Ułatwia implementację wzorca MVVM w WPF (np. `ObservableObject`, `RelayCommand`), redukując ilość boilerplate code. |
| **xUnit / NUnit** | Framework do testów jednostkowych i integracyjnych, co pozwala na zapewnienie niezawodności reguł biznesowych. |
| **FluentValidation** | Biblioteka do walidacji danych wejściowych po stronie logiki biznesowej, co zwiększa bezpieczeństwo i spójność danych. |
| **ASP.NET Core Identity** | Zarządzanie użytkownikami, hashowanie haseł, autoryzacja – kluczowe dla bezpieczeństwa danych medycznych. |

---

## Autorzy

*Krystian Janusz, Paulina Kaduk, Jakub Habdas, Mateusz Kozieł*
