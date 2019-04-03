using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIE
{
    public class ArtistNameMutations
    {
        public ArtistNameMutations()
        {
            FirstLevelArtistNameMutations= LoadMutations();

        }


        public IEnumerable<string> GetArtistNameMutated(string rawArtistNamePart)
        {
            var artistNamePart = rawArtistNamePart.Trim();
            if (FirstLevelArtistNameMutations.Keys.Contains(artistNamePart))
            {
                foreach (var artistNameMutation in FirstLevelArtistNameMutations[artistNamePart])
                {
                    if (!string.IsNullOrEmpty(artistNameMutation))
                        yield return artistNameMutation.Trim();
                }
            }
            else
            {
                yield return artistNamePart.Trim();
            }
        }


        private Dictionary<string, IEnumerable<string>> LoadMutations()
        {
            return new Dictionary<string, IEnumerable<string>>
            {
                {"Albert King with Stevie Ray Vaughan", new[] {"Albert King", "Stevie Ray Vaughan"}},
                {"Mino, Pedro Ricardo", new[] {"Mino", "Pedro Ricardo"}},
                {"Avey Tare, Panda Bear, Deakin", new[] {"Avey Tare", "Panda Bear", "Deakin"}},
                {
                    "Brian Wilson, Van Dyke Parks",
                    new[] {"Brian Wilson", "Van Dyke Parks"}
                },
                {"Brian Wilson And Van Dyke Parks", new[] {"Brian Wilson", "Van Dyke Parks"}},
                {"Morris Levy, William Davis", new[] {"Morris Levy", "William Davis"}},
                {"Mike Diamond, Adam Yauch, Adam Horovitz", new[] {"Mike Diamond", "Adam Yauch", "Adam Horovitz"}},
                {
                    "Benny Carter, Roy Eldridge, Zoot Sims, Clark Terry",
                    new[] {"Benny Carter", "Roy Eldridge", "Zoot Sims", "Clark Terry"}
                },
                {"Michael Sandison, Marcus Eoin", new[] {"Michael Sandison", "Marcus Eoin"}},
                {"Bobby Caldwell and Michael Lington", new[] {"Bobby Caldwell", "Michael Lington"}},
                {"Percy Jones, Brian Eno", new[] {"Percy Jones", "Brian Eno"}},
                {"Fred Frith, Brian Eno", new[] {"Fred Frith", "Brian Eno"}},
                {"Brian Eno David Byrne", new[] {"Brian Eno", "David Byrne"}},
                {
                    "Peter Cottontale, Cam for J.U.S.T.I.C.E League",
                    new[] {"Peter Cottontale", "Cam for J.U.S.T.I.C.E League"}
                },
                {
                    "The Marvels + Lucinda Williams",
                    new[] {"The Marvels", "Lucinda Williams"}
                },
                {"Freida Nerangis　- Britt Britton", new[] {"Freida Nerangis", "Britt Britton"}},
                {"Bert Reid - Raymond Reid", new[] {"Bert Reid", "Raymond Reid"}},
                {"Diplo, Angger Dimas", new[] {"Diplo", "Angger Dimas"}},
                {"Johnny Jewel with Symmetry", new[] {"Johnny Jewel", "Symmetry"}},
                {"Johnny Jewel with Heaven", new[] {"Johnny Jewel", "Heaven"}},
                {"Johnny Jewel with Glass Candy", new[] {"Johnny Jewel", "Glass Candy"}},
                {"Papathanassiou, Evanghelous \"Vangelis\"", new[] {"Vangelis", "Papathanassiou Evanghelous"}},
                {"Amber Coffman, Dave Longstreth", new[] {"Amber Coffman", "Dave Longstreth"}},
                {"Damon Albarn, Brian Eno", new[] {"Damon Albarn", "Brian Eno"}},
                {
                    "Thebe Kgositsile, Christopher Wallace, Osten Harvey, Burt Bacharach, Erica Wright, Hal David, Garry Glenn, Karriem Riggins",
                    new[]
                    {
                        "Thebe Kgositsile", "Christopher Wallace", "Osten Harvey", "Burt Bacharach", "Erica Wright",
                        "Hal David", "Garry Glenn", "Karriem Riggins"
                    }
                },
                {
                    "Eivor Palsdottir, Trondur Bogason",
                    new[] {"Eivor Palsdottir", "Trondur Bogason"}
                },
                {
                    "Eivor Palsdottir, Trondur Bogason, Maya Hawie",
                    new[] {"Eivor Palsdottir", "Trondur Bogason", "Maya Hawie"}
                },
                {
                    "Eivor Palsdottir, Trondur Bogason, Marjun Syderbo Kjalnes",
                    new[] {"Eivor Palsdottir", "Trondur Bogason", "Marjun Syderbo Kjalnes"}
                },
                {
                    "Eivor Palsdottir, Trondur Bogason, Randi Ward",
                    new[] {"Eivor Palsdottir", "Trondur Bogason", "Randi Ward"}
                },
                {
                    "Eivor Palsdottir, Trondur Bogason, Hogni Lisberg",
                    new[] {"Eivor Palsdottir", "Trondur Bogason", "Hogni Lisberg"}
                },
                {
                    "Jonathan Higgs, Jeremy Pritchard, Alexander Robertshaw",
                    new[] {"Jonathan Higgs", "Jeremy Pritchard", "Alexander Robertshaw"}
                },
                {"Basstaxx and Melvin Jakobs", new[] {"Basstaxx", "Melvin Jakobs"}},
                {"Geo Da Silva, Sean Norvis, Brazylero", new[] {"Geo Da Silva", "Sean Norvis", "Brazylero"}},
                {"TJR, Fatman Scoop, Reece Low", new[] {"TJR", "Fatman Scoop", "Reece Low"}},
                //to be fixed
                { "Attila Syah & Gamma (Ind)", new[] {"Attila Syah", "Gamma (Ind)"}},
                {
                    "Niels Henning, Orsted Pedersen",
                    new[] {"Niels Henning", "Orsted Pedersen"}
                },
                {"Depeche Mode, Daniel Miller", new[] {"Depeche Mode", "Daniel Miller"}},
                {
                    "Gary Nesta Pine and Dollerman",
                    new[] {"Gary Nesta Pine", "Dollerman"}
                },
                {"Inaya Day and China Ro", new[] {"Inaya Day", "China Ro"}},
                {
                    "Mario Fargetta and Montecarlo Five",
                    new[] {"Mario Fargetta", "Montecarlo Five"}
                },
                {"Pedro Del Mar Meets Sasha Da", new[] {"Pedro Del Mar", "Sasha Da"}},
                {"Dave Swarbrick, Simon Nicol", new[] {"Dave Swarbrick", "Simon Nicol"}},
                {"Dave Swarbrick, Richard Thompson", new[] {"Dave Swarbrick", "Richard Thompson"}},
                { "Curtis Hook, Morris Summer", new[] {"Curtis Hook", "Morris Summer"}},
                {"Jet Harris And Tony Meehan", new[] {"Jet Harris", "Tony Meehan"}},
                {"Julian Casablancas + The Voidz", new[] {"Julian Casablancas", "The Voidz"}},
                {"Robert Cray with Albert Collins", new[] {"Robert Cray", "Albert Collins"}},
                {
                    "Jimmy Rogers With Ronnie Earl And The Broadcasters",
                    new[] {"Jimmy Rogers", "Ronnie Earl And The Broadcasters"}
                },
                {"Lonnie Mack with Stevie Ray Vaughan", new[] {"Lonnie Mack", "Stevie Ray Vaughan"}},
                {
                    "The James Taylor Quartet feat. Alison Limerick",
                    new[] {"The James Taylor Quartet", "Alison Limerick"}
                },
                {"Barry K Sharpe And Diana Brown", new[] {"Barry K Sharpe", "Diana Brown"}},
                {"Nicola Conte And Micatone", new[] {"Nicola Conte", "Micatone"}},
                {"Herbie Hancock And Chaka Khan", new[] {"Herbie Hancock", "Chaka Khan"}},
                {"James Hardway and Billie Holiday", new[] {"James Hardway", "Billie Holiday"}},
                {"The Peter Malik Group and Norah Jones", new[] {"The Peter Malik Group", "Norah Jones"}},
                {
                    "Guido Spumante, Pepe Spumante",
                    new[] {"Guido Spumante", "Pepe Spumante"}
                },
                {"ursula rucker with little louie vega", new[] {"ursula rucker", "little louie vega"}},
                {"Charlie Byrd And Ken Peplowski", new[] {"Charlie Byrd", "Ken Peplowski"}},
                {"Ella Fitzgerald And Joe Pass", new[] {"Ella Fitzgerald", "Joe Pass"}},
                {"Tony Bennett With Bill Evans", new[] {"Tony Bennett", "Bill Evans"}},
                {"Mel Torme` With The Marty Paich Dek-Tette", new[] {"Mel Torme", "The Marty Paich Dek-Tette"}},
                {"Carmen McRae With George Shearing", new[] {"Carmen McRae", "George Shearing"}},
                {"Ernestine Anderson With George Shearing", new[] {"Ernestine Anderson", "George Shearing"}},
                {
                    "Yolando Vergara, Enrique Lazaga and Juan Crespo",
                    new[] {"Yolando Vergara", "Enrique Lazaga", "Juan Crespo"}
                },
                {
                    "Tommy McCook, Richard Ace, The Skatalites And Disco Height",
                    new[] {"Tommy McCook", "Richard Ace", "The Skatalites", "Disco Height"}
                },
                {"Jackie Mittoo And Ernest Ranglin", new[] {"Jackie Mittoo", "Ernest Ranglin"}},
                {"Karl Bryan And The Afrokats", new[] {"Karl Bryan", "The Afrokats"}},
                {"Jackie Mittoo And Brentford Rockers", new[] {"Jackie Mittoo", "Brentford Rockers"}},
                {"Karl Bryan And Count Ossie", new[] {"Karl Bryan", "Count Ossie"}},
                {
                    "Ben Webster With The Duke Ellington Orchestra",
                    new[] {"Ben Webster", "The Duke Ellington Orchestra"}
                },
                {
                    "Miles Davis With The Metronome All Stars",
                    new[] {"Miles Davis", "The Metronome All Stars"}
                },
                {
                    "Stan Getz With Woody Herman And His Orchestra",
                    new[] {"Stan Getz", "Woody Herman And His Orchestra"}
                },
                {"Muvva 'Guitar' Hubbard & The Stompers", new[] {"Muvva 'Guitar' Hubbard", "The Stompers"}},
                {"Lafayette Thomas w. Al Price Orchestra", new[] {"Lafayette Thomas", "Al Price Orchestra"}},
                //to be fixed
                { "Antonio Carlos e Jocafi", new[] {"Antonio Carlos Jocafi"}},
                //to be fixed
                { "The Muskyteers (aka The Silvertones)", new[] {"The Muskyteers", "The Silvertones"}},
                {"Hannibal and the Sunrise Orchestra", new[] {"Hannibal", "The Sunrise Orchestra"}},
                {
                    "Motihar Trio/Schweizer Trio/Schoof/Wilen",
                    new[] {"Motihar Trio", "Schweizer Trio", "Schoof", "Wilen"}
                },
                {
                    "Rusty Bryant, Bernard Purdie, Boogaloo Joe Jones, Jimmy Carter",
                    new[] {"Rusty Bryant", "Bernard Purdie", "Boogaloo Joe Jones", "Jimmy Carter"}
                },
                {"Kanye West, Elton John",new []{ "Kanye West","Elton John"} },
                {"Kanye West, I. Schmidt, M. Karoli, J. Liebezeit, K. Suzuki", new []{ "Kanye West","I. Schmidt","M. Karoli","J. Liebezeit","K. Suzuki" } },
                {"Kanye West & Eric Hudson",new []{ "Kanye West","Eric Hudson" } },
                {"Eric San, Emiliana Torrini", new []{ "Eric San","Emiliana Torrini" } },
                {"Hajime Wakai, Shiho Fujii, Mahito Yokota, Takeshi Hama,",new []{ "Hajime Wakai","Shiho Fujii","Mahito Yokota","Takeshi Hama"} },

                {"Hajime Wakai, Shiho Fujii, Mahito Yokota, ,",new []{ "Hajime Wakai","Shiho Fujii","Mahito Yokota"} },
                {"Hajime Wakai, Shiho Fujii, Mahito Yokota,",new []{ "Hajime Wakai","Shiho Fujii","Mahito Yokota"} },
                {"Hajime Wakai, Shiho Fujii, Mahito Yokota",new []{ "Hajime Wakai","Shiho Fujii","Mahito Yokota"} },
                {"Kosuke Yamashita, Chad Seiter", new[]{ "Kosuke Yamashita","Chad Seiter"} },
                {"Mahito Yokota, Hajime Wakai", new []{ "Mahito Yokota","Hajime Wakai"} },
                {"Kronos Quartet, Laurie Anderson",new[]{ "Kronos Quartet","Laurie Anderson" } },
                {"Sasha Sloan, Kyrre Gorvell-Dahll", new []{ "Sasha Sloan","Kyrre Gorvell-Dahll"} },
                {"John Newman, Kyrre Gorvell-Dahll",new []{ "John Newman","Kyrre Gorvell-Dahll"} },
                {"Oliver Nelson, Bonnie McKee, Kyrre Gorvell-Dahll", new []{ "Oliver Nelson","Bonnie McKee","Kyrre Gorvell-Dahll" } },
                {"Kyrre Gorvell-Dahll, Ryan Tedder",new []{ "Kyrre Gorvell-Dahll","Ryan Tedder"} },
                {"Kyrre Gorvell-Dahll, Martin Johnson, Linda Karlsson, Sonny Gustafsson", new []{ "Kyrre Gorvell-Dahll","Martin Johnson","Linda Karlsson","Sonny Gustafsson"} },
                {"Ari Leff, James Prentiss Sunderland, Brett A. Hite",new []{ "Ari Leff","James Prentiss Sunderland","Brett A. Hite"} },
                {"Andrew Goldstein, Ari Leff",new []{ "Andrew Goldstein","Ari Leff"} },
                {"Ari Leff, Tinashe C. Sibanda",new []{ "Ari Leff","Tinashe C. Sibanda"} },
                {"Ari Leff, Michael Matosic", new []{ "Ari Leff","Michael Matosic" } },
                {"Ari Leff, Jesse Saint John Geller",new []{ "Ari Leff","Jesse Saint John","Geller Pollack"} },
                {"Ari Leff, Michael Matosic, Michael Ross Pollack", new []{ "Ari Leff","Michael Matosic","Michael Pollack"} },
                {"Ari Leff, Michael Ross Pollack",new []{ "Ari Leff","Michael Pollack"} },
                {"Lindstrom And Solale",new []{ "Lindstrom","Solale" } },
                {"Louis Armstrong, Danny Kaye", new []{ "Louis Armstrong","Danny Kaye" } },
                {"Mark Kozelek and Sean Yeaton", new []{ "Mark Kozelek","Sean Yeaton" } },
                {"Mike Sarne And Wendy Richards", new []{ "Mike Sarne","Wendy Richards" } },
                {"Miles Davis, Bill Evans", new []{ "Miles Davis","Bill Evans" } },
                {"Mihai Cernea, Gabriel Litvin", new []{ "Mihai Cernea","Gabriel Litvin" } },
                {"Mono & World's End Girlfriend", new []{ "Mono","World's End Girlfriend" } },
                {"Mono und Nikitaman", new []{ "Mono","Nikitaman" } },
                {"Ivan J Neville,Cris Scott Jacobs", new []{ "Ivan J Neville","Cris Scott Jacobs" } },
                {"Ivan J Neville,Anders Osborne",new []{ "Ivan J Neville","Anders Osborne" } },
                {"George Clinton,Clarence Haskins,Edward Hazel,William Nelson", new []{ "George Clinton","Clarence Haskins","Edward Hazel","William Nelson" } },
                {"Steven Stewark,Ivan J Neville", new []{ "Steven Stewark","Ivan J Neville" } },
                {"Nick Drake and Gabrielle Drake", new []{ "Nick Drake","Gabrielle Drake" } },
                {"Joe Dukie And DJ Fitchie", new []{ "Joe Dukie","DJ Fitchie" } },
                {"Nina Attal, Anthony Honnet", new []{ "Nina Attal","Anthony Honnet" } },
                {"Nina Attal, Anthony Honnet, Levin Deger", new []{ "Nina Attal","Anthony Honnet","Levin Deger" } },
                {"Andre Benjamin, Antwan Patton, David Sheats, Lola Mitchell", new []{ "Andre Benjamin","Antwan Patton","David Sheats","Lola Mitchell"} },
                //to be fixed
                { "A. Benjamin, A. Patton", new []{ "Andre Benjamin","Antwan Patton"} },
                {"A. Benjamin, A. Patton, D. Sheats, Erin Johnson", new []{ "Andre Benjamin","Antwan Patton","David Sheats","Erin Johnson","Louis Freese" } },
                {"Andre Benjamin, Antwan Patton, David Sheats", new []{ "Andre Benjamin","Antwan Patton","David Sheats"} },
                {"Andre Benjamin, Antwan Patton", new []{ "Andre Benjamin","Antwan Patton"} },
                {"Antwan Patton, David Sheats", new []{ "Antwan Patton","David Sheats"} },
                {"Andre Benjamin, Antwan Patton, David Sheats", new []{ "Andre Benjamin","Antwan Patton","David Sheats" } },
                {"Andre Benjamin, Antwan Patton, David Sheats, Patrick Brown", new []{ "Andre Benjamin","Antwan Patton","David Sheats","Patrick Brown"} },
                {"Andre Benjamin, Antwan Patton", new []{ "Andre Benjamin","Antwan Patton"} },
                {"A. Benjamin, A. Patton, Corey Andrews, D. Sheats, John E.E. Smith", new []{ "Andre Benjamin","Antwan Patton","Corey Andrews","David Sheats","John E.E. Smith"} },
                {"Ike And Tina Turner", new []{ "Ike Turner","Tina Turner" } },
                {"Richard Burnett and Leonard Rutherford", new []{ "Richard Burnett","Leonard Rutherford" } },
                {"Buster Carter and Preston Young", new [] { "Buster Carter","Preston Young" } },
                {"Charlie Poole and the North Carolina Ramblers", new []{ "Charlie Poole","The North Carolina Ramblers" } },
                {"Secos And Molhados", new []{ "Secos","Molhados" } },
                {"Jaime Alem And Nair De Candia", new []{ "Jaime Alem","Nair De Candia" } },
                {"Gilberto Gil With Gal Costa", new []{ "Gilberto Gil","Gal Costa" } },
                {"Al Bowlly with Ray Noble & His Orchestra", new []{ "Al Bowlly","Ray Noble & His Orchestra" } },
                {"Guy Lecluyse, Hakob Ghasabian", new []{ "Guy Lecluyse","Hakob Ghasabian" } },
                {"Enrico Macias, Pauline", new []{ "Enrico Macias","Pauline" } },
                {"Pierre Richard, Line Renaud", new []{ "Pierre Richard","Line Renaud" } },
                {"Alain Souchon, Pierre Souchon, Ours, Cecile Hercule", new []{ "Alain Souchon","Pierre Souchon","Ours","Cecile Hercule" } },
                {"Camille Cerf, Maeva Coucke, Elodie Gossuin, Rachel Legrain Trapani, Iris Mittenaere", new []{ "Camille Cerf","Maeva Coucke","Elodie Gossuin","Rachel Legrain Trapani","Iris Mittenaere" } },
                {"Yolande Moreau, Franck Vandecasteele", new []{ "Yolande Moreau","Franck Vandecasteele" } },
                {"Rosa Passos and Ron Carter", new []{ "Rosa Passos","Ron Carter" } },
                {"Allen Ravenstine, David Thomas, Scott Krauss, Tom Herman, Tony Maimone", new []{ "Allen Ravenstine","David Thomas","Scott Krauss","Tom Herman","Tony Maimone" } },
                {"Peter Kater - R. Carlos Nakai", new []{ "Peter Kater","R. Carlos Nakai" } },
                {"Richard Barbieri, Steven Wilson", new []{ "Richard Barbieri","Steven Wilson" } },
                {"Richard Barbieri, Colin Edwin, Gavin Harrison, Steven Wilson", new []{ "Richard Barbieri","Colin Edwin","Gavin Harrison","Steven Wilson" } },
                {"Albano and Romina Power", new []{ "Albano","Romina Power" } },
                {"Cold War Kids, Bishop Briggs", new []{ "Cold War Kids","Bishop Briggs" } },
                {"ZZ Ward, Fantastic Negrito", new []{ "ZZ Ward","Fantastic Negrito" } },
                {"Bebe Rexha and Digital Farm Animals", new []{ "Bebe Rexha","Digital Farm Animals" } },
                {"Zedd (feat. Alessia Cara)", new []{ "Zedd","Alessia Cara" } },
                {"Sa4, Gzuz, Bonez MC", new []{ "Sa4","Gzuz","Bonez MC"} },
                {"LX, Sa4", new []{ "LX","Sa4" } },
                {"ATB with F51 (", new []{ "ATB","F51" } },
                {"Jayti-Moktar", new []{ "Jayti","Moktar" } },
                {"David Visan et Carlos Campos", new []{ "David Visan","Carlos Campos" } },
                {"Prodigy,  Kirillich", new []{ "The Prodigy","Kirillich"} },
                {"T-Fest X Скриптонит", new []{ "T-Fest","Skryptonit" } },
                //to be fixed
                { "Natalia Avelon Feat.Bela B.", new []{ "Natalia Avelon","Bela B." } },
                {"The Supremes + The Temptations", new []{ "The Supremes","The Temptations" } },
                {"The Supremes + The Four Tops", new []{ "The Supremes","The Four Tops" } },
                //to be fixed
                { "Roller Idol Feat.Bonfeel Electro Band", new []{ "Roller Idol","Bonfeel Electro Band" } },
                {"Dr.Crack Feat.Italo Business", new []{ "Dr.Crack","Italo Business" } },
                {"Cristina Manzano Feat.Kristian Conde", new []{ "Cristina Manzano","Kristian Conde" } },
                {"Will The Funkboss, Claire Andson", new []{ "Will The Funkboss","Claire Andson" } },
                //change the following one for avoiding single fs
                { "Jordan F feat. Le Cassette", new []{ "Jordan F ","Le Cassette" } },
                //to be fixed
                { "Sting Feat.Cheb Mami", new []{ "Sting","Cheb Mami" } },
                {"Eros Ramazzotti Feat.Cher", new []{ "Eros Ramazzotti","Cher" } },
                {"Eros Ramazzotti Feat.Tina Turner", new []{ "Eros Ramazzotti","Tina Turner" } },
                {"OneRepublic Feat.Timbaland", new []{ "OneRepublic","Timbaland" } },
                {"Arash Feat.Helena", new []{ "Arash","Helena" } },
                {"Youssou N'Dour Feat.Neneh Cherry", new []{ "Youssou N'Dour","Neneh Cherry" } },
                {"Eros Ramazzotti Feat.Anastacia", new []{ "Eros Ramazzotti","Anastacia" } },
                {"Akon Feat.Snoop Dogg", new []{ "Akon","Snoop Dogg" } },
                {"Nick Cave And The Bad Seeds Feat.Kylie Minogue", new []{ "Nick Cave & The Bad Seeds","Kylie Minogue" } },
                {"Chris Rea Feat.Elton John", new []{ "Chris Rea","Elton John" } },
                {"Enrique Iglesias Feat.Whitney Houston", new []{ "Enrique Iglesias","Whitney Houston" } },
                {"Flipsyde Feat.Piper", new []{ "Flipsyde","Piper" } },
                {"Sting Feat.Craig David", new []{ "Sting","Craig David" } },
                {"Eros Ramazzotti Feat.Patsy Kensit", new []{ "Eros Ramazzotti","Patsy Kensit" } },
                {"Nelly Furtado Feat.Juanes", new []{ "Nelly Furtado","Juanes" } },
                {"Blue Feat.Elton John", new []{ "Blue","Elton John" } },
                {"Oren Lavie Feat.Vanessa Paradis", new []{ "Oren Lavie","Vanessa Paradis" } },
                {"E-Type, Mud, Jonas \"Joker\" Berggren", new []{ "E-Type","Mud","Jonas \"Joker\" Berggren" } },
                {"R. Carlos Nakai And James DeMars", new []{ "R. Carlos Nakai","James DeMars" } },
                {"Eric Adiele, Hakeem Lewis, Patrick Morales", new []{ "Eric Adiele","Hakeem Lewis","Patrick Morales" } },
                {"David Porter, Eric Adiele, Hakeem Lewis, Isaac Hayes, Patrick Morales", new []{"David Porter","Eric Adiele","Hakeem Lewis","Isaac Hayes","Patrick Morales"}},
                {"Archie Marshall, Eric Adiele, Hakeem Lewis, Patrick Morales", new []{ "Archie Marshall","Eric Adiele","Hakeem Lewis","Patrick Morales" } },
                {"Clement 'Coxson' Dodd, Eric Adiele, Hakeem Lewis, Patrick Morales", new []{ "Clement 'Coxson' Dodd","Eric Adiele","Hakeem Lewis","Patrick Morales" } },
                {"Ray Guntrip and Tina May", new []{ "Ray Guntrip","Tina May" } },
                {"Ron Carter with Herbie Hancock and Tony Williams",new []{ "Ron Carter","Herbie Hancock","Tony Williams" } },
                {"Ron Carter - Herbie Hancock - Tony Williams",new[]{ "Ron Carter","Herbie Hancock","Tony Williams" } },
                {"Mike Kerr, Dave Murray, Ben Thatcher", new []{ "Mike Kerr","Dave Murray","Ben Thatcher" } },
                {"Maxi Priest and Shaggy", new []{ "Maxi Priest","Shaggy" } },
                {"Centory and Trey D", new []{ "Centory","Trey D" } },
                {"Gary Moore And Phil Lynott", new []{ "Gary Moore","Phil Lynott" } },
                //take out the bands they are not playing
                { "Tommy Shaw (Styx) & Steve Lukather (Toto)", new []{ "Tommy Shaw","Steve Lukather" } },
                {"Adrian Belew (King Crimson) & Alan White (Yes)", new []{ "Adrian Belew","Alan White" } },
                {"Gary Green (Gentle Giant) & Robbie Krieger (The Doors)", new []{ "Gary Green","Robbie Krieger" } },
                {"Robben Ford, Steve Porcaro (Toto) & Aynsley Dunbar (Journey)", new []{ "Robben Ford","Steve Porcaro","Aynsley Dunbar" } },
                {"Adrian Belew (King Crimson) & Keith Emerson (Elp)", new []{ "Adrian Belew","Keith Emerson" } },
                {"Bill Bruford (Yes, King Crimson), Tony Levin (King Crimson), Edgar Winter", new []{ "Bill Bruford","Tony Levin","Edgar Winter" } },
                {"Rick Wakeman (Yes), Steve Howe (Yes)", new []{"Rick Wakeman","Steve Howe"}},
                {"Eric And Mondrek Muchena", new []{ "Eric Muchena","Mondrek Muchena" } },
                //to be fixed
                { "Gavin Friday backed by Antony", new []{ "Gavin Friday","Antony" } },
                {"Robin Holcomb, Julie Christensen & Perla Batalla", new []{ "Robin Holcomb","Julie Christensen","Perla Batalla" } },
                {"Nick Cave, Perla Batalla", new []{ "Nick Cave","Perla Batalla"} },
                {"John Lennon and The Plastic On", new []{ "John Lennon","The Plastic Ono Band" } },
                //to be fuxed
                { "Mitch Ryder&The Detroit Wheels", new []{ "Mitch Ryder","The Detroit Wheels" } },
                {"Nick Cave And The Bad Seeds", new []{ "Nick Cave & The Bad Seeds" } },
                {"MC5, Dennis Thompson, Fred Smith, Michael Davis, Rob Tyner, Wayne Kramer", new []{ "MC5","Dennis Thompson","Fred Smith","Michael Davis","Rob Tyner","Wayne Kramer" } },
                {"Jefferson Turner, Michael Geggus, Vincent Riordan", new []{ "Jefferson Turner","Michael Geggus","Vincent Riordan" } },
                {"Gordon Ogilvie, Jake Burns", new []{ "Gordon Ogilvie","Jake Burns" } },
                {"David Brock, Robert Calvert", new []{ "David Brock","Robert Calvert" } },
                {"Josefin Ohrn + The Liberation", new []{ "Josefin Ohrn","The Liberation" } },
                {"Acid Mothers Temple and the Cosmic Inferno", new []{ "Acid Mothers Temple","The Cosmic Inferno" } },
                {"Sam Yahel, Mike Moreno, Ari Hoenig, Seamus Blake", new []{ "Sam Yahel","Mike Moreno","Ari Hoenig","Seamus Blake" } },
                {"Peter Thorn and Jim Goodin", new []{ "Peter Thorn","Jim Goodin" } },
                {"c rayz walz + kosha dillz", new []{ "c-rayz walz","kosha dillz" } },
                {"Kate Tucker and the Sons of Sweden", new []{ "Kate Tucker","The Sons of Sweden" } },
                {"Kid Congo and the Pink Monkey Birds", new []{ "Kid Congo and The Pink Monkey" } },
                {"Bryan Scary and The Shredding Tears", new []{ "Bryan Scary","The Shredding Tears" } },
                //to be fixed
                { "DADDY (Will Kimbrough & Tommy Womack)", new []{ "DADDY","Will Kimbrough","Tommy Womack" } },
                {"Emily Grace and Matthew Gardner", new []{ "Emily Grace","Matthew Gardner" } },
                {"Ezra Furman and the Harpoons", new []{ "Ezra Furman","The Harpoons" } },
                {"Gordon Gano and The Ryan Brothers", new []{ "Gordon Gano","The Ryan Brothers" } },
                {"Holly Golightly and the Brokeoffs", new []{ "Holly Golightly","The Brokeoffs" } },
                {"Jack Oblivian and the Tennessee Tearjerkers", new []{ "Jack Oblivian","The Tennessee Tearjerkers" } },
                {"Johnny Goudie and The Little Champions", new []{ "Johnny Goudie","The Little Champions" } },
                {"Mr Lewis and The Funeral 5", new []{ "Mr. Lewis","The Funeral 5" } },
                {"Shawn Sahm and the Tex Mex Experience", new []{ "Shawn Sahm","The Tex Mex Experience" } },
                {"Steve Burns (and The Struggle)", new []{ "Steve Burns","The Struggle" } },
                {"Steve Goldberg and the Arch Enemies", new []{ "Steve Goldberg","The Arch Enemies" } },
                {"The Krayolas and The West Side Horns", new []{ "The Krayolas","The West Side Horns" } },
                {"Capsula [performing The Rise and Fall of Ziggy Stardust and the Spiders from Mars]", new []{ "Capsula" } },
                {"Charli XCX + SOPHIE", new []{ "Charli XCX","SOPHIE" } },
                {"ChihiroYamazaki+ROUTE14band", new []{ "Chihiro Yamazaki","ROUTE 14 band" } },
                {"Covet (with Yvette Young)", new []{ "Covet","Yvette Young" } },
                {"David Borne and Jason Martin Featuring Kree Harrison", new []{ "David Borne","Jason Martin","Kree Harrison" } },
                {"Eric Dingus Presents TSO", new []{ "Eric Dingus","TSO" } },
                //to be fuxed
                { "Illionaire Records (Dok2 & The Quiett)", new []{ "Illionaire Records","Dok2 & The Quiett" } },
                {"Lock Johnson & The IvorY JeaN BanD", new []{ "Lock Johnson","The IvorY JeaN BanD" } },
                {"Mr. Lewis and The Funeral 5", new []{ "Mr. Lewis","The Funeral 5" } },
                {"Nyce Lutchiano x Stevo The Weirdo", new []{ "Nyce Lutchiano","Stevo The Weirdo" } },
                {"Ruby Velle and The Soulphonics", new []{ "Ruby Velle","The Soulphonics" } },
                {"Ryan Lofty x Rich Jones", new []{ "Ryan Lofty","Rich Jones" } },
                {"Teddy Thompson and Kelly Jones", new []{ "Teddy Thompson","Kelly Jones" } },
                {"William Harries Graham & the Painted Redstarts", new []{ "William Harries Graham","The Painted Redstarts" } },
                {"Y2K w/ Special Guest Lil Aaron", new []{ "Y2K","Lil Aaron" } },
                {"Zion.T with The Session", new []{ "Zion.T","The Session" } },
                {"Lauren Ashley and The Trainwreckers", new []{ "Lauren Ashley","The Trainwreckers" } },
                {"Moses Boyd - Solo X", new []{ "Moses Boyd","Solo X" } },
                {"Nitti Gritti x Tascione B2B", new []{ "Nitti Gritti","Tascione" } },
                {"Sahad and The Nataal Patchwork", new []{ "Sahad","The Nataal Patchwork" } },
                {"Wesley Jensen and The Penny Arcade", new []{ "Wesley Jensen","The Penny Arcade" } },
                {"Eastern Medicine Singers and Thor Harris", new []{ "Eastern Medicine Singers","Thor Harris" } },
                {"Chris Colepaugh and the Cosmic", new []{ "Chris Colepaugh","The Cosmic Crew" } },
                {"Chris Colepaugh and the Crew", new []{ "Chris Colepaugh","The Cosmic Crew" } },
                {"Ryan McPhun and The Ruby Suns", new []{ "Ryan McPhun","The Ruby Suns" } },
                {"Scott Miller & The Commonwealt", new []{ "Scott Miller","The Commonwealth" } },
                {"IV Thieves (Nic Armstrong)", new []{ "IV Thieves","Nic Armstrong" } },
                {"Chris Colepaugh and the Cosmic Crew", new []{ "Chris Colepaugh","The Cosmic Crew" } },
                //to be fixed
                { "Adam Franklin (of Swervedriver) & Bolts of Melody", new []{ "Adam Franklin","Bolts of Melody" } },
                {"Wayne Kramer, Billy Bragg, Tom Morello, Chris Shiflett and others tba", new []{ "Wayne Kramer","Billy Bragg","Tom Morello","Chris Shiflett" } },
                {"Keelay and The Park with Ragen Fykes", new []{ "Keelay and The Park","Ragen Fykes" } },
                {"BangNRecords / Bartholomew Boyz / Sup Crew Mob", new []{ "BangNRecords","Bartholomew Boyz","Sup Crew Mob" } },
                {"Bucky Dolla and M-Burb The Captain", new []{ "Bucky Dolla","M-Burb The Captain" } },
                {"Corey Paul and Reconcile (Frontline Movement)", new []{ "Corey Paul","Reconcile","Frontline Movement" } },
                {"Dosti Music Project / Produced by Bang on a Can's Found Sound Nation", new []{ "Dosti Music Project","Bang","Can's Found Sound Nation" } },
                {"Dos Santos: Anti-Beat Orquesta", new []{ "Dos Santos","Anti-Beat Orquesta" } },
                {"Jeff Stuart and The Hearts", new []{ "Jeff Stuart","The Hearts" } },
                {"Jesse Harris with Star Rover", new []{ "Jesse Harris","Star Rover" } },
                {"Joel Laviolette and Rattletree", new []{ "Joel Laviolette","Rattletree" } },
                {"Melissa Brooks and The Aquadolls", new []{ "Melissa Brooks","The Aquadolls" } },
                {"My Education/Theta Naught: Sound Mass", new []{ "My Education","Theta Naught","Sound Mass" } },
                {"Casii Stephan and the Midnight Sun", new []{ "Casii Stephan","the Midnight Sun" } },
                {"Joecephus and The George Jonestown Massacre", new []{ "Joecephus","The George Jonestown Massacre" } },
                //to be fuxed
                { "Peter Lewis (Moby Grape) & Arwen Lewis", new []{ "Peter Lewis","Arwen Lewis" } },
                {"Pile (Rick Maguire solo)", new []{ "Pile","Rick Maguire" } },
                {"Rich Minus Tribute with Eric Hisaw, Neal Walker, Mitch Webb, George Ensle, Steve Tombstone, Sheli Coe, Ricky Broussard, and more", new []{ "Rich Minus Tribute","Eric Hisaw","Neal Walker","Mitch Webb","George Ensle","Steve Tombstone","Sheli Coe","Ricky Broussard" } },
                {"The M.G.'s With The Mar-Keys", new []{ "The M.G.'s","The Mar-Keys" } },
                {"Alan Reeves, Phil Steele", new []{ "Alan Reeves","Phil Steele" } },
                {"Arthur Fiedler And The Boston Pops", new []{ "Arthur Fiedler","The Boston Pops" } },
                {"Carmine Coppola And Francis Ford Coppola", new []{ "Carmine Coppola","Francis Ford Coppola" } },
                {"John Morris And Mel Brooks", new []{ "John Morris","Mel Brooks" } },
                {"Christophe Heral, Billy Martin", new []{"Christophe Heral","Billy Martin"}},
                {"Frank Sullivan, James Michael Peterik", new []{ "Frank Sullivan","James Michael Peterik" } },
                {"Christophe Heral, Ennio Morricone", new []{ "Christophe Heral","Ennio Morricone" } },
                {"Trust, Anthrax", new []{"Trust","Anthrax"}},
                {"Paul Turrell, Ben Henderson", new []{ "Paul Turrell","Ben Henderson" } },
                {"Trent Reznor and Atticus Ross", new []{ "Trent Reznor","Atticus Ross" } },
                {"Cris Velasco and Sascha Dikiciyan", new []{ "Cris Velasco","Sascha Dikiciyan" } },
                {"Clint Mansell and Sam Hulick", new []{ "Clint Mansell","Sam Hulick" } },
                {"Luke Silas, James DeVito, Peter Berkman" ,new []{ "Luke Silas","James DeVito","Peter Berkman"} },
                {"Luke Silas, Peter Berkman, Ary Warnaar", new []{ "Luke Silas","Peter Berkman","Ary Warnaar" } },
                {"James Bagshaw, Thomas Warmsley", new []{ "James Bagshaw","Thomas Warmsley" } },
                {"Tess Parks And Anton Newcombe", new []{ "Tess Parks","Anton Newcombe" } },
                {"V.Rossi-T.Ferro", new []{ "Vasco Rossi","Tullio Ferro" } },
                {"V.Rossi-M.Solieri", new []{ "Vasco Rossi","M. Solieri" } },
                {"Shellback,Max Martin,Justin Timberlake", new []{ "Shellback","Max Martin","Justin Timberlake" } },
                {"Gerry Wexler,Solomon Burke,Bert Berns", new []{ "Gerry Wexler","Solomon Burke","Bert Berns" } },
                {"Will Smith,Fred Washington,Patrice Rushen,Terri Mc Faddin", new []{ "Will Smith","Fred Washington","Patrice Rushen","Terri Mc Faddin" } },
                {"James Page,Rober Plant,Mark Curry,John Bonham,Sean Combs,Cedric Protheroe,Eric Levisalles", new []{ "James Page","Robert Plant","Mark Curry","John Bonham","Sean Combs","Cedric Protheroe","Eric Levisalles" } },
                {"Huey Lewis,Christopher Hayes,John Colla", new []{ "Huey Lewis","Christopher Hayes","John Colla" } },
                {"Wynton Kelly Trio, Wes Montgomery", new []{ "Wynton Kelly Trio","Wes Montgomery" } },
                {"Georgia Hubley-Ira Kaplan-James McNew", new []{ "Georgia Hubley","Ira Kaplan","James McNew" } },
                {"Aaron Freeman aka Gene Ween, Michael Melchiondo Jr. aka Dean Ween aka Mickey Moist", new []{ "Aaron Freeman aka Gene Ween","Michael Melchiondo Jr. aka Dean Ween aka Mickey Moist" } },
                {"Cheeky Cheeky And The Nosebleeds", new []{ "Cheeky Cheeky","The Nosebleeds" } },
                {"Chris Catalena And The Native Americans", new []{ "Chris Catalena","The Native Americans" } },
                {"David Bavas and the Down Comfo", new []{ "David Bavas","he Down Comforter" } },
                {"DJ Jigue and El Menor", new []{ "DJ Jigue","El Menor" } },
                {"Djuma Soundsystem & Westerby", new []{ "Djuma Soundsystem","Westerby" } },
                {"Donald Byrd And 125th Street NYC", new []{ "Donald Byrd","125th Street NYC" } },
                {"Felix Leiter x Tank & Cheetah", new []{ "Felix Leiter","Tank & Cheetah" } },
                {"Haggeman and Marco Rochowski", new []{ "Haggeman","Marco Rochowski" } },
                {"Ian Dury and The Blockheads", new []{ "Ian Dury","The Blockheads" } },
                {"Jovelli x Feathwr", new []{ "Jovelli","Feathwr" } },
                {"Judy Collins and Ari Hest", new []{ "Judy Collins","Ari Hest" } },
                {"Justin Sherburn and Montopolis", new []{ "Justin Sherburn","Montopolis" } },
                {"LEEHAHN X VANJESS", new []{ "LEEHAHN","VANJESS" } },
                {"Louie Vega, Patrick Adams", new []{ "Louie Vega","Patrick Adams"} },
                {"Mars, Enois Scroggins", new []{ "Mars","Enois Scroggins" } },
                {"Mitch Webb and the Swindles", new []{ "Mitch Webb","The Swindles" } },
                {"Mitsuto Suzuki, Naoshi Mizuta", new []{ "Mitsuto Suzuki","Naoshi Mizuta" } },
                {"Mud, E-Type", new []{ "Mud","E-Type" } },
                {"Niels Henning Orsted Pedersen & Tania Maria", new []{ "Niels Henning Orsted Pedersen","Tania Maria" } },
                {"Otto Schwarzfischer und Adi Stahuber mit iher Original Oktoberfest Blasmusik", new []{ "Otto Schwarzfischer","Adi Stahuber","Original Oktoberfest Blasmusik" } },
                {"Rom Di Prisco and Peter Chapman", new []{ "Rom Di Prisco","Peter Chapman" } },
                {"Scott Weiland and The Wildabouts", new []{ "Scott Weiland","The Wildabouts" } },
                {"Shubzilla x Bill Beats", new []{ "Shubzilla","Bill Beats" } },
                {"SIZE-S ✖ XenontiX", new []{ "SIZE-S","XenontiX" } },
                {"SOB X RBE", new []{ "SOB","RBE" } },
                {"PSK-13, Point Blank", new []{ "PSK-13","Point Blank"} },
                {"John Wesley Harding and Eugene Mirman's Cabinet of Wonders with special guests", new []{ "John Wesley Harding","Eugene Mirman's Cabinet of Wonders" } },
                {"Tan Dun with Yo", new []{ "Tan Dun","Yo" } },
                {"T-Fest X Skryptonit", new []{ "T-Fest","Skryptonit" } },
                {"The Explosives with Peter Lewis and Stu Cook", new []{ "The Explosives","Peter Lewis","Stu Cook" } },
                {"Tom Hingley and Kelly Wood", new []{ "Tom Hingley","Kelly Wood" } },
                {"Uyarakq x Peand-eL", new []{ "Uyarakq","Peand-eL" } },
                {"William and Versey Smith", new []{ "William Smith", "Versey Smith" } },
                {"Yoshitaka Suzuki, Naoshi Mizuta", new []{ "Yoshitaka Suzuki","Naoshi Mizuta" } },
                {"Andre Benjamin, Antwan Patton, Brian Loving, Carlton, Corey Andrews, David Sheats, Mahone", new []{ "Andre Benjamin","Antwan Patton","Brian Loving","Carlton","Corey Andrews","David Sheats","Mahone" } },
                {"Bang N Records/Bartholomew Boyz/Sup Crew Mob", new []{ "Bang N Records","Bartholomew Boyz","Sup Crew Mob" } },
                {"Bonez MC, Raf Camora, Gzuz", new []{ "Bonez MC","Raf Camora","Gzuz" } },
                {"Dido Armstrong, Pascal Gabriel",new []{ "Dido","Pascal Gabriel"}},
                {"Emilie Satt,Jean-Karl Lucas", new []{ "Emilie Satt","Jean-Karl Lucas" } },
                {"Guillen, Ignacio Canut", new []{ "Guillen, Ignacio Canut" } },
                {"Masashi Hamauzu, Naoshi Mizuta", new []{ "Masashi Hamauzu","Naoshi Mizuta" } },
                {"Matias Cena Amelia Castillo y Matias Cena (3 4 12 14)", new []{ "Matias Cena", "Amelya Castillo" } },
                {"Royston Wood And Heather Wood", new []{ "Royston Wood","Heather Wood" } },
                {"Sophia Pfister, Dave Alvin", new []{ "Sophia Pfister","Dave Alvin" } },
                {"Sophia Pfister, White Buffalo Stands", new []{ "Sophia Pfister","White Buffalo Stands" } },
                {"Whiteside and Lil Kane", new []{ "Whiteside","Lil Kane" } },
                {"Wolfgang Klingler, Thomas Heimes, Hans Christian Mittag", new []{ "Wolfgang Klingler","Thomas Heimes","Hans Christian Mittag" } },
                //from features
                //aply the split for featured also
                { "Idol/Steve Stevens", new []{ "Billy Idol","Steve Stevens" } },
                {"Driver Pegasus Warning", new [] {"Busdriver","Pegasus Warning"} },
                {"Jeremiah Jae Aesop Rock",new []{ "Jeremiah Jae","Aesop Rock"} },
                {"Great Dane Open Mike Eagle", new []{ "Great Dane","Open Mike Eagle" } },
                {"Mike Gao VerBS", new []{ "Mike Gao","VerBS" } },
                {"Guilty Simpson and Paradime", new []{ "Guilty Simpson","Paradime" } },
                {"Wilderness x A-line", new []{ "Wilderness","A-line" } },
                {"Niklas Gustavsson Ludvig Holm", new []{ "Niklas Gustavsson","Ludvig Holm"} },
                {"Airscape Sarah McLachlan", new []{ "Airscape","Sarah McLachlan" } },
                {"Kill The Pain DJ Shadow", new []{ "Kill The Pain","DJ Shadow"} },
                {"Adam Brooks and Will Rees", new []{ "Adam Brooks","Will Rees" } },
                {"Harry Romero and Jose Nunez", new []{ "Harry Romero","Jose Nunez" } },
                {"Young Pulse And ATN", new []{ "Young Pulse","ATN" } },
                {"fatboy slim DJ Scar", new []{ "fatboy slim","DJ Scar" } },
                {"Abdul Shyllon Marathon Men", new []{ "Abdul Shyllon","Marathon Men" } },
                {"Sacha Six Series", new []{ "Sacha","Six Series" } },
                {"Manuelle DJ Fopp Jazzy", new []{ "Manuelle","DJ Fopp Jazzy" } },
                {"Rav x Rekcahdam", new []{ "Rav","Rekcahdam" } },
                {"b-bandj dj krush", new []{ "b-bandj","dj krush" } },
                {"Onanistic James Murphy And Eric Broucek", new []{ "James Murphy","Eric Broucek" } },
                {"YCP Beno and Signor Benedick the Moor", new []{"YCP Beno","Signor Benedick the Moor" } },
                {"Los Macorinos, Omara Portuondo & Eugenia Leon", new []{ "Los Macorinos","Omara Portuondo","Eugenia Leon" } },
                {"Melanie Pain Ian McCulloch", new []{ "Melanie Pain","Ian McCulloch" } },
                {"Nadeah Miranda Barry Adamson", new []{ "Nadeah Miranda","Barry Adamson" } },
                {"Marina Celeste Terry Hall", new []{ "Marina Celeste","Terry Hall" } },
                {"Lil Kim And Puff Daddy", new []{ "Lil Kim","Puff Daddy" } },
                {"Jagged Edge And Avery Storm", new []{ "Jagged Edge","Avery Storm" } },
                {"Fabolous And Busta Rhymes", new []{ "Fabolous","Busta Rhymes" } },
                {"Ja Rule And Ralph Tresvant", new []{ "Ja Rule","Ralph Tresvant" } },
                {"Big Gipp and Ludacris", new []{ "Big Gipp","Ludacris" } },
                {"Killer Mike and Jay-Z", new []{ "Killer Mike","Jay-Z" } },
                {"Khujo Goodie and Cee-Lo", new []{ "Khujo Goodie","CeeLo Green" } },
                {"the East Side Boyz and Mello", new []{ "The East Side Boyz","Mello" } },
                {"Sleepy Brown and Jazze Pha", new []{ "Sleepy Brown","Jazze Pha" } },
                {"Quincey And Sonance", new []{ "Quincey","Sonance" } },
                {"Witchdoctor and Phonte", new []{ "Witchdoctor","Phonte" } },
                {"Matisyahu +Kosha Dillz", new []{ "Matisyahu","Kosha Dillz" } },
                {"Chance The Rapper and Buddy", new []{ "Chance The Rapper","Buddy" } },
                {"Christo + Childish Major", new []{ "Christo","Childish Major" } },
                {"Cashtro + JusMoni", new []{ "Cashtro","JusMoni" } },
                {"Redd and Lil Keke", new []{ "Redd","Lil Keke" } },
                {"Harrison Sands and Copper King", new []{ "Harrison Sands","Copper King" } },
                {"KNDRX x SuperLove", new []{ "KNDRX","SuperLove" } },
                {"Genesis Blu Chelsea Mariah", new []{ "Genesis Blu","Chelsea Mariah"} },
                {"Ceschi and Sammus", new []{ "Ceschi","Sammus" } },
                {"Kam Moye and Ragen Fykes", new []{ "Kam Moye","Ragen Fykes" } },
                {"Simon Katz and Jared Dietch", new []{ "Simon Katz","Jared Dietch" } },
                {"Rittz and mc chris", new []{ "Rittz","mc chris" } },
                {"Yankee x Trilogy", new []{ "Yankee","Trilogy" } },
                {"Kid Koala and The Protomen", new []{ "Kid Koala","The Protomen" } },
                {"Shabazz Palaces + Cashtro", new []{ "Shabazz Palaces","Cashtro" } },
                {"s.al and randal bravery", new []{ "s.al","randal bravery" } },
                {"Big Boi Sleepy Brown", new []{ "Big Boi","Sleepy Brown" } },
                {"Nappy Roots Martin Luther", new []{ "Nappy Roots","Martin Luther" } },
                {"Jungle Josh x JROB", new []{ "Jungle Josh","JROB" } },
                {"Dr 2Fat Lxt", new []{ "Dr 2Fat","Lxt" } },
                {"End Troy Baker and Courtnee Draper", new []{ "Troy Baker","Courtnee Draper" } },
                {"WDR Bigband Gianluigi Trovesi", new []{ "WDR Bigband","Gianluigi Trovesi" } },
                {"Charles-Tone Brenda Boykin", new []{ "Charles-Tone","Brenda Boykin" } },
                {"Sleepy Wonder And Gunjan", new []{ "Sleepy Wonder","Gunjan" } },
                {"Elvis Costello and Lou Reed", new []{ "Elvis Costello","Lou Reed" } },
                {"Marie Fisker/Steen Jorgensen", new []{ "Marie Fisker","Steen Jorgensen" } },
                {"Chamillionaire Krayzie Bone", new []{ "Chamillionaire","Krayzie Bone" } },
                {"John Talabot and Pional Blinded", new []{ "John Talabot","Pional Blinded" } },
                {"Son Little Djuma Soundsystem", new []{ "Son Little","Djuma Soundsystem" } },
                {"Red Eye Tiger Stripes", new []{ "Red Eye","Tiger Stripes" } },
                {"BAM speeka", new []{ "BAM","speeka" } },
                {"BAM dubspeeka", new []{ "BAM","dubspeeka" } },
                {"Tamara Dey Deetron Up", new []{ "Tamara Dey","Deetron Up" } },
                {"Son Little Cord Labuhn", new []{ "Son Little","Cord Labuhn" } },
                {"Faithless Dido", new []{ "Faithless","Dido" } },
                {"Joss Stone Van Hunt", new []{ "Joss Stone","Van Hunt" } },
                {"Lxt Dr 2Fat", new []{ "Lxt","Dr 2Fat" } },
                {"Rebel Kleff and Jehst", new []{ "Rebel Kleff","Jehst" } },
                {"A Filetta, Paolo Fresu, Daniele di Bonaventura", new []{ "A Filetta","Paolo Fresu","Daniele di Bonaventura" } },
            };

        }

        public Dictionary<string, IEnumerable<string>> FirstLevelArtistNameMutations { get; }
    }
}
