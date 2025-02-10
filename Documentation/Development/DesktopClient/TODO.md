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
	- [ ] This issue takes place in couple of places:
		- [ ] Loading screen.
		- [ ] Switching between pages. In this case if one page has been loaded previously. Time required to open this page again is smaller.
	- [ ] I don't know if this bug is related to that but while rapid clicking on couple of pages, program crashes. 
	- [ ] Pagination 'Available pages' index is invalid from time to time. For example it shows that only 1 page is available but in fact there are more pages.
	- [ ] Go to 'Preview' operation execute on Project/Component/Purchase creates weird visual effect. Tab is changing but it appears that this action is not quick enough and background of this tab is visible for some time.

# Improvements:
- [ ] All of the items on 'Home' page should be filtered as following - latest changes on top.
- [ ] After clicking item on 'Home' page, user should be navigated to selected item's right location.
- [ ] User's name is cut in 'Home' page.
- [ ] Implement:
	- [ ] Settings
	- [ ] Night-mode 
	- [ ] About
	- [ ] Report bug
	- [ ] User settings
	- [ ] Delete 'Tracking' tab from menu.
