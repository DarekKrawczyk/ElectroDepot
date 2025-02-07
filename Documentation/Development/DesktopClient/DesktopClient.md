- [x] Przygotowanie zdjęcia na tło. Teraz jest ok ale jak sie rozciągnie ekran to przez to że nie jest foremne to sie rozciąga dziwnie i nic sie nie da zrobić. Chyba najlepiej będzie zrobić zdjęcie na rozdzielczość fullHD i tyle. Wybierz zdjęcie 'Background.png' z Assetów.
- [ ] Dobranie jakiś kolorów co będą posowały do sekcji.
	- [ ] Zaimplementowanie palety Light/Dark dla okna. Przez wykorzystanie FluentTheme aplikacja 'dziedziczy' wygląd po systemie. Można to troche zmodyfikować: https://docs.avaloniaui.net/docs/guides/styles-and-resources/how-to-use-theme-variants
- [x] Zrobienie tego gradientu dla sekcji żeby wyglądało tak jak w tym modern UI.
- [x] Zmiany w bazie danych, trzeba będzie dodać pola dla użytkowników takie jak 'Name', może też 'Surname' żeby potem wyświetlić to na ekranie głównym.
- [x] Pobranie obecnej daty i wyświetlenie na ekranie.
- [ ] 'Components' tab:
	- [ ] Collection: 
		- [ ] Kontrolki z filtrowaniem nie działają. Dlatego że teraz elementy wyświetlane są za pomocą 'Pagingu'
		- [ ] Kolorowanie 'DataGrid' dla komponentów nie działa prawidłowo, powinno być co 2 item a po zastosowaniu filtrowania psuje sie.
		- [ ] Trzeba zaimplementować funkcjonalności 'ContextMenu' dla Componentów, ale to dopiero po refaktoryzacji kart 'Add'/'Preview'.
		- [ ] Dodanie 'Context menu' dla każdego elementu w DataGrid. Powinien zawierać opis komponentu.
		- [x] Przy dodaniu Supplierów do contextmenu jest taki problem że wszyscy sklejają sie do jednego MenuItem i nie wiem jak ich rozdzielić na osobne. Póki co to może być na sztywno przypisane. Jednak sie udało poprzez nadpisanie wyglądu, zobacz 'ComponentsPageView.axaml'.
		- [ ] Po zmianie wyświetlanych elementów w DataGrid zrobiło się troche pusto. Może da się coś dołożyć? Może dostęność DatasheetPDF albo Footprint.
		- [ ] Po dodaniu Tooltipa na row's w componentach jest jakuś być i jak już raz sie włączy tooltip to będzie cały czas widoczny jak przejedzie sie na pozostałe elementy. Powinien sie odświeżać za każdym razem jak sie najedzie na immy row.
	- [ ] Add:
	- [ ] Preview:
		- [ ] Póki co jest funkcjonalność aktualizacji zdjęcia i kategorii komponentu ale frontend do tego jest wyłączony bo:
			- Jeżeli chodzi o zdjęcie to nie ma co dodawać tej funkcjonalności jak nie ma nowego systemu przechowywania zdjęć. Jak zostanie zaimplementowany to sie doda.
			- Zmiane kategorii można zrobić ale tak sie coś krzaczy z powodu niewłaściwych ID komponentu. Proste do naprawy.
		- [ ] Dodać przechodzenie do Project/Purchase z widoku Preview komponentu. Ale to dopiero wtedy jak zostaną zrefatoryzowane te strony. 
		- [ ] Generowanie raportu - potrzebny jest pomysł jak to ma wyglądać. (Póki co: dodatkowe okno z wyświetlonym PDF'em raportu. Przydają sie taki funkcjonalności jak zapisz na dysku/drukuj. Taki raport może zawierać wszystkie pola komponentu oraz spis w których projektach został wykorzystany i kiedy/gdzie został kupiony).
		- [ ] Funkcjonalności pobrania opisu z digikey czekają aż podepnie sie API z DigiKeya.
		- [ ] 
- [ ] Dodać przycisk do zmiany z tryby nocnego/dziennego.
- [ ] Trzeba poświecić troche czasu jeżeli chodzi o 'MessageBox' bo nie jest to takie proste jak sie wydaje. Póki co zostawiam tak jak jest.
## Components:
- [x] Dodać FullDescription/DatasheetLink/Image do modelu komponentu
- [x] Błędne dane z owned i used components. Prawdopodobnie spowodowane tym że są złe dane testowe.
- [x] Dokończyć dodawanie zdjęć
- [ ] Opisać w readme dlaczego nie działą podłączenia handlerów do eventów w TemplatedControls - link to issue na gicie w tellefonie
- [ ] Obsługa dodania/modyfikowania komponentu na serverze
- [ ] Obsługa poprawności dodawania/modyfikowania componentów
- [ ] Kowersja z Bitmap do byte[] powinna być pomijana bo widziałem że dla tego samego brazka generowane są różne arrayki. Najlepiej operować tylko na byte array do operacji a Bitmap będzie tylko do wyświetlania
- [ ] Gdy nowy komponent zostanie dodany to trzeba zrobić kilka rzeczy:
	- [ ] Aktualizacja listy komponentów
	- [ ] Wyświetlenie komunikatu że został dodany prawidłowo
	- [ ] Przenisienie użytkownika na karte 'Preview'
- [ ] Dodać double click do wyboru komponentów
- [ ] Naprawić bug z wyświetlaniem kategorii które przewija się od początku
- [ ] Stworzyć osobną tabele na producentów 'Manufacturers'
- [ ] Dodanie przejścia do 'Project'/'Purchase' w zakładce Component/Preview/(Purchased/Used in Projects)
- [ ] Zaimplementować jakiś nowy system do przechowywania zdjęć, coś co będzie wspierało monitorowanie odniesień i usuwanie gdy nic nie korzysta ze zdjęcia, dziele odniesień do tych samych zdjęć. Archiwizacja zdjęć np. do zip itd...




## ...
https://componentsearchengine.com/part-view/NHD-0420E2Z-FSW-GBW/Newhaven%20Display modele komponentow



# Avalonia toturials

---

## Jak dodawć nowe customowe elementy oparte na innych
Jak chcesz zrobić jakiś nowy element UI, i chciałbyś żeby dziedziczył po czymś żeby zachować jakieś konkretne własności możesz postąpić tak.
Przykład:
 - Stwórz nowy TemplateControl i w klasie zmien dziedziczenie na takie jakie chcesz np. Button
 - W pliku .axml aby połączyć się z własnościami tej klasy po której dziedziczysz użyj: Command="TemplateBinding Command"
 - Wszystko co wsadzisz ControlTemplate będzie wyświetlane na ekranie.
 - Aby wykorzystać prawidłowo Command to w .axml musi być coś co to ztriggeruje. Nie wiem czy nie ma czegoś takiego dla pozostałych elementów.

---

## Jak stworzyć liste elementów z modelu na UI
 - Wiadomo musi być model który ma takiej propertisy co można zmapować do stworzonego tamplate 
 - Jak masz problem taki że nie ma takiego widoku który chcesz któy umozlliwia podłączenie się do niego np. UniformGrid. Trzeba skorzystać z 'ItemsControl' tak jak w 'HomePageView.axaml'
 - https://docs.avaloniaui.net/docs/reference/controls/itemscontrol

---

## Tworzenie templated control i binding
- Jak stworzysz templated control i jakimiś bindingami do np. List<string> którą przekazujesz to jak stworzysz dodatkowy konstruktor to te wartości się nie przypiszą, nie wiem czemu tak jest. Ale najlepiej reagować na eventy, zobacz jak to jest zrobione w '/CustomControls/ImageSelector.axaml.cs'. Tak samo jak chcesz podłączyć się do jakiejś kontrolki w tym templated control zobacz jak to zrobiłem w metodzie 'OnApplyTemplate()' w taki spoób to działa a inaczej średnio. Ogólnie lepiej jest tak robić bo jak robiłem to w sposób z user control to bindowały sie tylko komnedy ale już dane sie nie bindowały.

