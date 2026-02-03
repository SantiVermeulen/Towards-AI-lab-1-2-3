# VRXP-towards-ai
## Labo-01
### Uitleg

#### Uitleg variabelen
![alt text](/Screenshots/Screenshot1.png)

Hier zien we een aantal variabelen:

```csharp
public Transform[] waypoints;
```
Het doel van deze variabele is om de info van de waypoints (palm bomen) bij te houden (rotatie, positie, schaal). Hier hebben we dus vooral de positie nodig van de waypoints.

```csharp
public float speed = 5.0f;
```
Deze variabele bepaalt de snelheid van de tank. We kunnen dit ook aanpassen met de inspector window.

```csharp
public float rotSpeed = 4.0f;
```
Deze variabele kan je afleiden van de benaming, het bepaalt de rotatiesnelheid van de tank, zoals de variabele hierboven kan je dit ook aanpassen met de inspector window.

```csharp
private int currentWaypoint = 0;
```
Deze variabelen houd eigelijk bij welke checkpoint de tank naartoe moet rijden, eigenlijk een soort interne teller.

#### Uitleg Werking code

![alt text](/Screenshots/Screenshot2.png)

Hier zien we de "main" code:

```csharp
Vector3 direction = waypoints[currentWaypoint].position - transform.position;
```
Deze stuk code bepaalt de richting dat de tank zich moet richten om naar de tank te gaan.

```csharp
Quaternion lookRotation = Quaternion.LookRotation(direction);
```
Deze stuk code creëert een rotatie-waarde die kijkt in de richting van de zojuist berekende direction vector.

```csharp
transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);
```
Dit werd al in de comments uitgelegd (screenshot)

```csharp
if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 2.0f)
```
Dit stukje code is eigenlijk een controle voor de afstand tussen de tank en de waypoints, als de afstand tussen de tank en de waypoint kleinder is dan 2.0 dan word het als aangekomen beschouwd.

```csharp
currentWaypoint++;
```
Telt de currentWaypoint variabele met 1 op, zodat de tank naar de volgende waypoint gaat.

```csharp
if (currentWaypoint >= waypoints.Length)
{
    currentWaypoint = 0;
}
```
Dit is loop dat ervoor zorgt dat als de tank aan de laatste waypoint geraakt, dan zal die terug vanaf 0 beginnen en terug opnieuw beginnen.

## Labo-02
### Uitleg
### algemene uitleg scripts

#### Script: Maze.cs
Dit script is verantwoordelijk voor het genereren en visualiseren van een doolhof. Dit script vormt de gridomgeving waarin het A*-algoritme zoekt naar het optimale pad.

#### Script: FindPathAStar.cs
Dit script implementeert de volledige logica van het A* pathfinding algoritme. Het vindt het kortst mogelijke pad tussen een startpunt en een eindpunt, rekening houdend met de muren die door Maze.cs zijn gedefinieerd. 

### Uitleg aangepaste / toegevoegde code

![alt text](/Screenshots/Screenshot3.png)
Hier hebben we de lijn code:
```csharp
    StartCoroutine(Searching()); 
```
Zoals in de screenshot vermeld, start die de coroutine searching.

![alt text](/Screenshots/Screenshot4.png)
Ik heb deze stuk code aangepast zoals te zien in de screenshot hierboven. Searching() zoekt stap voor stap naar de Goal, wacht even voor visualisatie, en zodra de Goal gevonden is, reconstrueert het het optimale pad en laat de Player dat pad automatisch volgen.

![alt text](/Screenshots/Screenshot5.png)
Ik heb deze stuk code toegevoegd. Deze coroutine verplaatst de Player cube stap voor stap (1 seconde per node) langs het berekende pad totdat het doel bereikt is.

### Tasks

task 2.1: Develop a path reconstruction method: Once the goal is reached, the algorithm works backward through the parent references to construct the optimal path from start to goal.
![alt text](/Screenshots/Screenshot6.png)

task2.2: Implement the system automically start the search once the start and end
goal is selected.
![alt text](/Screenshots/Screenshot7.png)
![alt text](/Screenshots/Screenshot8.png)

Your task2.3: Implement a method so the player will walk automatically (after theA*search) to the goal (use a coroutine, so you do 1 step per second)
![alt text](/Screenshots/Screenshot9.png)

## Labo-03

### Uitleg
#### Script: TankDriver.cs

In het algemeen wordt deze script gebruikt om de tank te laten bewegen naar het gekozen palmboom. 

##### variabelen

```csharp
public WPManager wpManager;
``` 
Deze variabele is nodig omdat de TankDriver hier de Graph (het netwerk van waypoints en verbindingen) vandaan haalt.

```csharp
public TMP_Dropdown dropdown;
```
Het script gebruikt dit om de keuzes (de waypoints) in te laden en om te detecteren wanneer de speler een nieuwe bestemming kiest.

```csharp
public float accuracy;
```
Dit is eigenlijk een variabele dat gebruikt wordt om te bepalen wanneer een tank dicht genoeg is bij een palmboom. In de eerste labo oefening hebben dit met een vaste variabele gedaan.

##### Functies

![alt text](/Screenshots/Screenshot10.png)

Deze eerste stuk word gebruikt voor het voorbereiden van de tank en de UI voor gebruik.

```csharp
graph = wpManager.graph;
```
Haalt de graph (het netwerk van alle waypoints en verbindingen) op uit de WPManager en slaat deze op in een variabele.

```csharp
dropdown.ClearOptions();
```
Maakt de dropdown lijst leeg. Dit is een goede gewoonte om te voorkomen dat er dubbele opties ontstaan als de scene opnieuw zou laden.

```csharp
foreach (GameObject wp in wpManager.waypoints)
{
    options.Add(wp.name);
}
```
Start een lus die elke GameObject in de waypoints lijst van de WPManager doorloopt en voegt de naam van de huidige waypoint (bijv. "Palm Tree 1") toe aan een tijdelijke lijst met options.

```csharp
dropdown.AddOptions(options);
```
Vult de UI dropdown met alle namen die in de options lijst zijn verzameld. De speler kan nu alle waypoints zien en selecteren.

```csharp
dropdown.onValueChanged.AddListener(delegate { DriveToDestination(); });
```
Dit is de belangrijkste stap. Het koppelt een event listenen aan de dropdown, dus op het moment dat een persoon op een palmboom klikt bij de dropdown dan zal de functie DriveToDestination() uitgevoerd worden.

![alt text](/Screenshots/Screenshot11.png)

Het script initieert een nieuwe A* zoektocht om een pad te berekenen. Deze functie laat de tank niet bewegen, het maakt alleen het plan.

```csharp
GameObject startNode = FindClosestWaypoint();
```
Roept de FindClosestWaypoint() functie aan om te bepalen welke waypoint op de map het dichtst bij de huidige positie van de tank is. Dit wordt het beginpunt van de route.

```csharp
GameObject endNode = wpManager.waypoints[dropdown.value];
```
Leest de index van de geselecteerde optie uit de dropdown. Als de gebruiker de derde optie kiest, is dropdown.value gelijk aan 2. Het gebruikt deze index om de corresponderende waypoint uit de waypoints lijst te halen. Dit wordt de eindbestemming.

```csharp
if (graph.AStar(startNode, endNode))
```
Roept de A* functie aan in de Graph klasse met het zojuist bepaalde startpunt en eindpunt. Deze functie doet alle complexe berekeningen en vult de pathList in de graph met het gevonden pad. De if-statement controleert of er wel een pad gevonden is.

```csharp
pathIndex = 0;
```
Als er een pad is gevonden, wordt pathIndex teruggezet naar 0. Dit is het signaal voor de Update() functie dat er een nieuw pad is en dat het bij het begin moet beginnen met volgen.

![alt text](/Screenshots/Screenshot12.png)
In deze functie word de tank daadwerkelijk bewogen langs de stappen van het pad dat door DriveToDestination() is berekend.

```csharp
if (graph.pathList.Count == 0 || pathIndex >= graph.pathList.Count)
```
Een veiligheidscheck. Als de pathList leeg is of als pathIndex voorbij het einde van de lijst is, stopt de functie onmiddellijk om fouten te voorkomen.

```csharp
currentNode = graph.getPathPoint(pathIndex);
```
Haalt de volgende waypoint-stap uit de pathList op basis van de huidige pathIndex. Dit is de directe bestemming waar de tank nu naartoe moet.

```csharp        
if (Vector3.Distance(...) < accuracy)
```
Controleert de afstand tussen de tank en de targetPosition. Als die afstand klein genoeg is, beschouwen we de waypoint als bereikt. (Zoals bij Labo-01)

![alt text](/Screenshots/Screenshot13.png)
Het bepalen van de meest logische start-waypoint voor het A*-algoritme.

```csharp 
float minDistance = float.MaxValue;
```
Initialiseert een variabele voor de kortste afstand met de hoogst mogelijke waarde.

```csharp 
foreach (GameObject wp in wpManager.waypoints)
```
Start een lus die alle waypoints in het netwerk doorloopt.

```csharp      
foreach (GameObject wp in wpManager.waypoints)
{
    float dist = Vector3.Distance(transform.position, wp.transform.position);
    if (dist < minDistance)
    {
        minDistance = dist;
        closest = wp;
    }
}
return closest;
```

1. Het start een zoektocht door de volledige lijst van waypoints te overlopen, één voor één.
2. Voor elke waypoint in de lijst meet het de afstand tot de tank.
3. Het vergelijkt deze afstand met de kortste afstand die het tot nu toe heeft onthouden.
4. Als een nieuwe waypoint dichterbij is, wordt deze onthouden als de nieuwe "dichtstbijzijnde".

## Labo-04
### Task Labo-04

Wat is een Navigation Mesh (NavMesh)?

Een Navigation Mesh (kortweg NavMesh) is als een slimme plattegrond voor je AI-personages. In plaats van te werken met een grid of een serie vaste waypoints, analyseert Unity de 3D-geometrie van je level (de grond, muren, obstakels) en genereert een versimpelde 2D-laag van polygonen over alle begaanbare gebieden.

Hoe wordt het gebruikt voor beweging?

Wanneer je een AI-agent (zoals je tank) een bestemming geeft, hoeft hij niet zelf een complex A*-algoritme uit te voeren. In plaats daarvan:

1. De agent kijkt naar de NavMesh.
2. Het vindt de snelste route over de polygonen van de NavMesh naar de bestemming, waarbij het automatisch obstakels ontwijkt die niet op de mesh liggen.
3. Het volgt dit pad.

Het grote voordeel is dat de AI zich vrij en intelligent kan bewegen binnen de blauwe gebieden, in plaats van vast te zitten aan een rigide lijn tussen twee waypoints.