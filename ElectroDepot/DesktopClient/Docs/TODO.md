# TODO:
## Components:
- [x] Doda� FullDescription/DatasheetLink/Image do modelu komponentu
- [x] B��dne dane z owned i used components. Prawdopodobnie spowodowane tym �e s� z�e dane testowe.
- [x] Doko�czy� dodawanie zdj��
- [ ] Opisa� w readme dlaczego nie dzia�� pod��czenia handler�w do event�w w TemplatedControls - link to issue na gicie w tellefonie
- [ ] Obs�uga dodania/modyfikowania komponentu na serverze
- [ ] Obs�uga poprawno�ci dodawania/modyfikowania component�w
- [ ] Kowersja z Bitmap do byte[] powinna by� pomijana bo widzia�em �e dla tego samego brazka generowane s� r�ne arrayki. Najlepiej operowa� tylko na byte array do operacji a Bitmap b�dzie tylko do wy�wietlania
- [ ] Gdy nowy komponent zostanie dodany to trzeba zrobi� kilka rzeczy:
	- [ ] Aktualizacja listy komponent�w
	- [ ] Wy�wietlenie komunikatu �e zosta� dodany prawid�owo
	- [ ] Przenisienie u�ytkownika na karte 'Preview'
- [ ] Doda� double click do wyboru komponent�w
- [ ] Naprawi� bug z wy�wietlaniem kategorii kt�re przewija si� od pocz�tku
- [ ] Stworzy� osobn� tabele na producent�w 'Manufacturers'
- [ ] Dodanie przej�cia do 'Project'/'Purchase' w zak�adce Component/Preview/(Purchased/Used in Projects)
- [ ] Zaimplementowa� jaki� nowy system do przechowywania zdj��, co� co b�dzie wspiera�o monitorowanie odniesie� i usuwanie gdy nic nie korzysta ze zdj�cia, dziele odniesie� do tych samych zdj��. Archiwizacja zdj�� np. do zip itd...
- [ ] 
---

## ...
https://componentsearchengine.com/part-view/NHD-0420E2Z-FSW-GBW/Newhaven%20Display modele komponentow



## Modify Code
<TabItem Header="Modify" IsEnabled="{Binding PreviewEnabled}">
          <!-- Main -->
          <Grid RowDefinitions="auto, *, auto, auto">
            <Grid Grid.Row="0" ColumnDefinitions="auto, 60*">
              <Grid Grid.Column="0">
                <!-- IMAGE -->
                <Image Source="avares://DesktopClient/Assets/Components_icon.png"
                       Width="128"
                       Margin="10"></Image>
              </Grid>
              <Grid Grid.Column="1">
                <!-- BASIC INFO LABELS-->
                <StackPanel>

                  <StackPanel Orientation="Horizontal"
                              Spacing="10">
                    <StackPanel Orientation="Vertical">
                      <Label FontWeight="Bold"
                             HorizontalAlignment="Left">Name</Label>
                      <TextBox HorizontalAlignment="Left"
                               Text="{Binding Modify_ComponentName}"></TextBox>
                    </StackPanel>
                    <StackPanel Orientation="Vertical">
                      <Label FontWeight="Bold"
                             HorizontalAlignment="Left">Manufacturer</Label>
                      <TextBox HorizontalAlignment="Left"
                               Text="{Binding Modify_ComponentManufacturer}"></TextBox>
                    </StackPanel>
                    <StackPanel Orientation="Vertical">
                      <Label FontWeight="Bold"
                             HorizontalAlignment="Left">Category</Label>
                      <ComboBox ItemsSource="{Binding Categories}"
                                HorizontalAlignment="Left"
                                SelectedItem="{Binding Modify_ComponentCategory}"></ComboBox>
                    </StackPanel>
                  </StackPanel>
                  <StackPanel Orientation="Vertical">
                    <Label FontWeight="Bold" HorizontalAlignment="Left">Datasheet</Label>
                    <StackPanel Orientation="Horizontal" Spacing="10">
                      <TextBox HorizontalAlignment="Left" Text="{Binding Modify_ComponentDatasheetLink}"></TextBox>
                      <Button IsEnabled="False" Command="{Binding Modify_PreviewCommand}">Preview</Button>
                    </StackPanel>
                  </StackPanel>
                </StackPanel>

              </Grid>
            </Grid>
            <Grid Grid.Row="1">
              <Grid.RowDefinitions>
                <!-- 30% for Short description -->
                <RowDefinition Height="3*" />
                <!-- 70% for Full description -->
                <RowDefinition Height="7*" />
              </Grid.RowDefinitions>

              <!-- Short Description Section -->
              <Grid Grid.Row="0">
                <Grid.RowDefinitions>
                  <!-- Short description Label -->
                  <RowDefinition Height="Auto" />
                  <!-- Short description TextBox -->
                  <RowDefinition Height="*" />
                </Grid.RowDefinitions>

                <Label Grid.Row="0" FontWeight="Bold" HorizontalAlignment="Left">Short description</Label>
                <TextBox Grid.Row="1"
                         TextWrapping="Wrap"
                         VerticalContentAlignment="Top"
                         HorizontalAlignment="Stretch"
                         VerticalAlignment="Stretch"
                         Text="{Binding Modify_ComponentShortDescription}"/>
              </Grid>

              <!-- Full Description Section -->
              <Grid Grid.Row="1">
                <Grid.RowDefinitions>
                  <!-- Full description Label -->
                  <RowDefinition Height="Auto" />
                  <!-- Full description TextBox -->
                  <RowDefinition Height="*" />
                </Grid.RowDefinitions>

                <Label Grid.Row="0" FontWeight="Bold" HorizontalAlignment="Left">Full description</Label>
                <TextBox Grid.Row="1"
                         Text="{Binding Modify_ComponentFullDescription}"
                         TextWrapping="Wrap"
                         VerticalContentAlignment="Top"
                         AcceptsReturn="True"
                         HorizontalAlignment="Stretch"
                         VerticalAlignment="Stretch" />
              </Grid>
            </Grid>

            <!-- Separator-->
            <Grid Grid.Row="2">
              <Border Height ="2"
                      Background="{DynamicResource UIMediumGray}"
                      Margin="0,10"
                      HorizontalAlignment="Stretch"/>
            </Grid>

            <Grid Grid.Row="3"
                  Margin="0 00 0 10">
              <StackPanel Orientation="Horizontal"
                          Spacing="10">
                <Button IsEnabled="{Binding Modify_CanModify}"
                        Command="{Binding Modify_ModifyCommand}">Modify</Button>
                <Button Command="{Binding Modify_ClearCommand}">Clear</Button>
                <Button Command="{Binding NavigateToCollectionCommand}">Cancel</Button>
              </StackPanel>
            </Grid>
          </Grid>
        </TabItem>