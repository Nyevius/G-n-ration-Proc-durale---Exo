## Table Of Contents

<details>
<summary>Details</summary>

  - [Introduction](#Introduction)
  - [Getting Started](#GettingStarted)
  - [Simple Room Placement](#SimpleRoomPlacement)
  - [Binary Space Partitioning](#BSP)
  - [Cellular Automata](#CellularAutomata)
  - [Noise Generator](#NoiseGenerator)
  - [Credits](#Credits)

</details>

## Introduction
Generation-Procedurale-Exo est un repo contenant des algorithmes permettant d'être utilisés en tant qu'outil pour faire de la génération procédurale sur Unity. 

## Getting Started
La première étape sera d'inclure ```ProceduralGridGenerator``` dans votre scène.  

<img src="Documentation/img1.png?raw=true"/>  

Ensuite, pour créer votre propre algorithme pour de la génération procédurale, il vous faudra créer un nouveau script héritant de ```ProceduralGenerationMethod```.  
Codez ensuite votre génération procédurale dans ce dernier et créez un asset en passant par ```Create > Procedural Generation Method > Le nom de votre script```.  

<img src="Documentation/asset.png?raw=true"/>  

Dans votre script, il vous faudra rentrer votre algorithme dans une fonction tel que :  
```
protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
{
    //Définition des variables

    // Check for cancellation
    cancellationToken.ThrowIfCancellationRequested();

    // Logique du code

    // Waiting between steps to see the result.
    await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
}
```

Pour finir, référencez le fichier ```.asset``` dans votre object ```ProceduralGridGenerator``` et vous aurez fini l'installation.  

<img src="Documentation/endsetup.png?raw=true"/>

## Simple Room Placement
Cet algorithme commence par générer des salles et les placer de façon aléatoire à condition qu'elles n'overlapent pas d'autres salles.  

<img src="Documentation/Simpleroomplacement.png?raw=true"/>

## BSP
Ce script va également générer des salles au début mais non pas de façon aléatoire cette fois. Il découpe la grille jusqu'à atteindre le nombre de nodes enfants demandé. Une fois ce nombre atteint, il va s'arrêter et instancier les salles à ces positions.
Ensuite, il va créer des couloirs pour relier les salles à leur parent.

<img src="Documentation/BSP.png?raw=true"/>

## Cellular Automata
Pour ce script, on abandonne le placement de salles. Cet algo sert à générer du terrain. On commence par du bruit, c'est-à-dire placer de façon aléatoire des tiles d'eau ou de terre. Ensuite on répère certaines conditions, si on les respecte (ici, plus de 4 cellules terre autour de celle qu'on regarde). Si la condition est respectée on passe la cellule qu'on regarde en terre, sinon on la passe en eau. On fait cette boucle pour chaque case de notre map.    

<img src="Documentation/Cellularautomata.png?raw=true"/>  

## Noise Generator  
Un autre algorithme de génération de terrain, mais plus précis cette fois. On passera par la librairie FastNoiseLite afin de générer une map de bruit. On le créé, lui assigne plusieurs paramètres et la librairie fait le reste il ne nous reste qu'à instancier nos tiles en fonction du résultat.  

<img src="Documentation/NoiseGenerator.png?raw=true"/>  

## Credits  
Merci en premier lieu à [RUTKOWSKI Yona](https://sites.google.com/view/rutkowski-yona) pour son enseignement et ses corrections.  

Un grand merci aussi à Cysharp pour [UniTask](https://github.com/Cysharp/UniTask) qui a permis de fluidifier l'éxecution dans Unity avec les threads.  

Et enfin merci aussi à Jordan Peck pour sa librairie [FastNoiseLite](https://github.com/Auburn/FastNoiseLite) pour la génération du bruit complexe pour la génération du terrain.
