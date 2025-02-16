I looks like docker is working correctly. But there are some improvements to make:
- Each API call calls constructor of controller, thus all of this code is called every time. ![[Pasted image 20250217195305.png]] 
  Lets do something with it.
- It is required to create simple to understand and workable ports binding in docker.