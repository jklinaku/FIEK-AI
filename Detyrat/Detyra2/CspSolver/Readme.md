Perdorimi

qka mund te nderroni eshte pjesa "cages" ne metoden main()

cages pranon liste te "Cage", nje Cage ka pjesen e shumes pra sa duhet te jete shuma e mbeledhur e qelizave qe i permban ai cage dhe pejsen e qelizave (pozicionen x,y) te cilat i takojne atij "cage"
shembull:
var cages = new List<Cage> {
            new Cage(2,   [(0,0)]),
            new Cage(22,  [(0,1), (1,1), (1,2)])
            }
new Cage(2,   [(0,0)]) => qeliza (0,0) duhet patjeter te jete 2
new Cage(22,  [(0,1), (1,1), (1,2)]) => shuma e qelizave (0,1) + (1,1) + (1,2) = 22
pastaj thjeshte behet run, aplikacioni eshte console app