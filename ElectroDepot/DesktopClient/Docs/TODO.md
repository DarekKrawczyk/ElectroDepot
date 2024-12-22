# TODO:
## Components:
- [x] Dodaæ FullDescription/DatasheetLink/Image do modelu komponentu
- [x] B³êdne dane z owned i used components. Prawdopodobnie spowodowane tym ¿e s¹ z³e dane testowe.
- [x] Dokoñczyæ dodawanie zdjêæ
- [ ] Opisaæ w readme dlaczego nie dzia³¹ pod³¹czenia handlerów do eventów w TemplatedControls - link to issue na gicie w tellefonie
- [ ] Obs³uga dodania/modyfikowania komponentu na serverze
- [ ] Obs³uga poprawnoœci dodawania/modyfikowania componentów
- [ ] Kowersja z Bitmap do byte[] powinna byæ pomijana bo widzia³em ¿e dla tego samego brazka generowane s¹ ró¿ne arrayki. Najlepiej operowaæ tylko na byte array do operacji a Bitmap bêdzie tylko do wyœwietlania
- [ ] Gdy nowy komponent zostanie dodany to trzeba zrobiæ kilka rzeczy:
	- [ ] Aktualizacja listy komponentów
	- [ ] Wyœwietlenie komunikatu ¿e zosta³ dodany prawid³owo
	- [ ] Przenisienie u¿ytkownika na karte 'Preview'
- [ ] Dodaæ double click do wyboru komponentów
- [ ] Naprawiæ bug z wyœwietlaniem kategorii które przewija siê od pocz¹tku
- [ ] Stworzyæ osobn¹ tabele na producentów 'Manufacturers'
- [ ] Dodanie przejœcia do 'Project'/'Purchase' w zak³adce Component/Preview/(Purchased/Used in Projects)
- [ ] Zaimplementowaæ jakiœ nowy system do przechowywania zdjêæ, coœ co bêdzie wspiera³o monitorowanie odniesieñ i usuwanie gdy nic nie korzysta ze zdjêcia, dziele odniesieñ do tych samych zdjêæ. Archiwizacja zdjêæ np. do zip itd...
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