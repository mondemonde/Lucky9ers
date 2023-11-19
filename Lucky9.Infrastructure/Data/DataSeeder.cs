using Lucky9.Application.Common.Interfaces;
using Lucky9.Domain.Entities;

namespace Lucky9.Infrastructure.Data;
    public class DataSeeder
    {
        private readonly IDataContext dataContext;

        public DataSeeder(IDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async void Seed()
        {


        //var client = new Client("xosiosiosdhad", "John", "Smith", "john@gmail.com", "+18202820232");
        //var client = new Player
        //{
        //    //Id = Guid.NewGuid(),
        //    FirstName = "John",
        //    LastName = "Smith",
        //    Email = "john@gmail.com",
        //   // PhoneNumber = "+18202820232"

        //};

        //   await  dataContext.Players.AddAsync(client);
        //    dataContext.SaveChanges();


        //cards
        var cards = dataContext.Cards.ToList();
        if(cards.Count < 40 ) {

            //clear all
          foreach( var card in cards ) {
             dataContext.Cards.Remove( card );
              await  dataContext.SaveChangesAsync();

            }

            // add new
            int cycle = 0;
            for ( int i = 1; i <= 40; i++ ) {

                cycle += 1;
                if(cycle > 10)
                    cycle = 1;


                if (i <= 10)
                    dataContext.Cards.Add(new Card {
                        Rank = cycle,
                        Suit = Domain.Enums.EnumCards.s.ToString()

                    });


                else if (i <= 20)

                    dataContext.Cards.Add(new Card
                    {
                        Rank = cycle,
                        Suit = Domain.Enums.EnumCards.f.ToString()

                    }); 

                else if (i <= 30)

                    dataContext.Cards.Add(new Card
                    {
                       Rank= cycle,
                        Suit = Domain.Enums.EnumCards.h.ToString()

                    });

                else if (i <= 40)

                    dataContext.Cards.Add(new Card
                    {
                       Rank = cycle,
                        Suit = Domain.Enums.EnumCards.d.ToString()

                    });

               await dataContext.SaveChangesAsync();

            }

        
        }


        }
    }

