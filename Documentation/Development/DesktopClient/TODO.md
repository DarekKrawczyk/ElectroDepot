#todo

# UI Refactoring:
- **Projects** page:
	- Projects tab:
		- [x] Idea for displaying projects should change. [[Projects collection layout]] ✅ 2025-02-08
		- [x] Add tab should change. [[Projects add tab refactoring]] ✅ 2025-02-08
		- [x] Preview/Edit tab should change as well. [[Edit tab refactoring]] ✅ 2025-02-0

# I think after this UI refactoring I can focus on some minor improvements and bug fixing.

# Known bugs:
- UI is being slowed down by operation. I think it has something to do with using only one core to render UI and execute business logic, for example retrieving data from database.
	- [x] This issue takes place in couple of places (Yeah this is caused by loading elements from DB, using async improves experience a lot): ✅ 2025-02-13
		- [x] Loading screen. ✅ 2025-02-13
		- [x] Switching between pages. In this case if one page has been loaded previously. Time required to open this page again is smaller. ✅ 2025-02-13
	- [x] I don't know if this bug is related to that but while rapid clicking on couple of pages, program crashes. ✅ 2025-02-13
	- [ ] Pagination 'Available pages' index is invalid from time to time. For example it shows that only 1 page is available but in fact there are more pages.
	- [ ] Go to 'Preview' operation execute on Project/Component/Purchase creates weird visual effect. Tab is changing but it appears that this action is not quick enough and background of this tab is visible for some time.

# Improvements:
- [x] All of the items on 'Home' page should be filtered as following - latest changes on top. ✅ 2025-02-11
- [x] After clicking item on 'Home' page, user should be navigated to selected item's right location. ✅ 2025-02-11
- [x] User's name is cut in 'Home' page. ✅ 2025-02-11
- [x] Implement: ✅ 2025-02-13
	- [x] Settings ✅ 2025-02-11
	- [x] Night-mode ✅ 2025-02-12
	- [x] About ✅ 2025-02-12
	- [x] Report bug ✅ 2025-02-12
	- [x] User settings ✅ 2025-02-11
	- [x] Delete 'Tracking' tab from menu. ✅ 2025-02-12
- [x] When server is unavailable user should be presented with such information. For now it just prints 'Invalid credentials'. ✅ 2025-02-13
- [x] Define some file that stores address and port of the server that application is connection to. ✅ 2025-02-13
- [x] Components: ✅ 2025-02-14
	- [x] 'Components' tab ✅ 2025-02-14
		- [x] Add refresh button ✅ 2025-02-14
		- [x] Pagination is bugged. ✅ 2025-02-14
		- [x] After adding new item and navigating to 'Components' tab. 'Current page' textbox displays its number like its left aligned. - It turns out that 'TextBlock' has some bugs. I have replaced it with 'Label' and now it works ok. ✅ 2025-02-14
		- [x] Disable sorting ✅ 2025-02-14
	- [x] 'Add' tab ✅ 2025-02-14
		- [x] When component is added, refresh 'Components' collection by adding new component to it. ✅ 2025-02-14
		- [x] Refresh collection. ✅ 2025-02-14
		- [x] Components can have same name. ✅ 2025-02-14
		- [x] Delete Watermarks for About and Description field ✅ 2025-02-14
	- [x] 'Preview/Edit' tab ✅ 2025-02-14
		- [x] Suppliers are not displayed. ✅ 2025-02-14
		- [x] Display only such 'Projects' and 'Purchases' that are related to 'User'. ✅ 2025-02-14
- [x] Projects: ✅ 2025-02-15
	- [x] 'Projects' tab ✅ 2025-02-15
		- [x] 'Name', 'Created' and 'Description' columns should be spreaded evenly. ✅ 2025-02-14
		- [x] Change 'Creating date' to 'Created' ✅ 2025-02-14
		- [x] While projects data is loading 'Clear' button is marked as clickable. Make it so be default this button is disabled, but upon first loaded data, it is enabled. Same with 'Purchases' tab collection ✅ 2025-02-14
	- [x] 'Add' tab ✅ 2025-02-15
		- [x] 'Avaiable' Components should have more implicit name like 'User's avaiable components' ✅ 2025-02-14
- [ ] Overall
	- [x] Change basic color of selection [[Change theme color]] ✅ 2025-02-15
- [ ] Create Installer
	- [x] Desktop ✅ 2025-02-16
		- [x] Windows ✅ 2025-02-15
			- [x] What about config files in users directory? It is managed by DesktopClient ✅ 2025-02-15
		- [x] Linux? ✅ 2025-02-16
		- [x] Mac? ✅ 2025-02-16
	- [ ] Server
		- [ ] Windows
- [ ] Server
	- [ ] Dataseeders
		- [ ] Default dataset should contain only Components/Categories/Suppliers tables.
		- [ ] Excel based DataSeeder in necessary.
		- [x] So i have refactored seeders a bit but. In case of Inline seeders 'Those defined in code' lets not touch them because refactoring them will take a lot of manual labor. It is better to focus on excel seeders, and in those seeders provide accurate Components-with-proper-image etc.. ✅ 2025-02-16
- [ ] CI/CD
	- [ ] Implement Pipeline for DesktopClient via github actions.

![[Pasted image 20250217013707.png]]