# Lucky9ers
This is a simple card game project to explore Clean Code architecture.

Here are the notable feature of the project:
* The project is based on Jason Talor's template as suggested in appendix
* I disabled the middle ware validation handler to implement my own validation - see comments on my BusinessRule folder below.
* I had some issues with the Identity server so I just implemnted a simple authentication to continue the project
	*about the issue it turns out I can create my own certificate to deploy in azure to fix the issue.
	* however I already finished my own jwt authentication so i continued without it.
*Following the Clean architecture, all logic runs in the applicaton layer and controllers are kept clean and Mediatr helps to achieve this.
*Interface implentation is in the infrastructure layer
* Dependecy direction is from  the domain model upto the web api  

* A noteworthy addition also is the BusinessRule folder inside the application layer.
I guess this is my own version of simplifying the programmer's life,
which I jokingly called Developers Driven Design. The main point of this folder is to encapsulate any important rules as discussed by the client. 
If you look closely to folder, you will find in there all the logic that is required by the project. This is actually the part you want to unit test and most likely you and your client will be looking for.

* I deployed this already in azure for tesing , this is playable now but there are still alot missing
   but for design evaluation this is enough for now.
	* https://lucky9ers.azurewebsites.net

### Aknowledgement: 
* CleanArchitecture template - https://github.com/jasontaylordev/CleanArchitecture
* Simple flip card Animations -  https://zoaibkhan.com/blog/angular-animations-create-a-card-flip-animation/
* Deck of cards library - https://github.com/deck-of-cards/deck-of-cards
* Authentication- https://www.youtube.com/playlist?list=PLc2Ziv7051bZhBeJlJaqq5lrQuVmBJL6A

