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

