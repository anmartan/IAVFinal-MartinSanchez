# IAVFinal-MartinSanchez
 
 Esta es la última práctica de la asignatura de Inteligencia Artificial para Videojuegos de la Universidad Complutense de Madrid.
 En ella, se ha desarrollado un prototipo de IA que resuelva un laberinto, sin saber a priori dónde están las salidas.

# Drive donde se encuentran los vídeos

Como parte de la documentación, se pueden ver los vídeos publicados en este [link](), para observar las pruebas que se han realizado y los resultados obtenidos.

# Descripción de la práctica

El famoso cuento infantil de Caperucita Roja nos sirve de inspiración para estudiar cómo se desenvuelve nuestra Inteligencia Artificial a la hora de resolver laberintos. Caperucita va a llevar a una cesta de comida para su abuela que, una vez más, ha vuelto a enfermar. 

Para llegar a casa de su abuela, debe atravesar un bosque repleto de árboles y por el que es fácil perderse. Por eso, Caperucita solo andará por los caminos que ya hay marcados en el suelo, sabiendo que, al menos uno, le llevará a su destino. 

El prototipo que se va a desarrollar, por tanto, es un laberinto en el que Caperucita se moverá, siguiendo una estrategia determinada, para intentar llegar a la salida del laberinto. 

El jugador puede controlar a Caperucita e intentar resolver el laberinto por su cuenta, o desactivar el movimiento del jugador y activar la IA, que hará el mismo trabajo. La IA tendrá varios niveles de “inteligencia”, entre los que el jugador puede escoger. En cada uno de estos niveles, el algoritmo empleado es más sofisticado.

Es importante recalcar que la IA no sabe dónde está la salida del laberinto, por lo que no puede utilizar algoritmos como A* o Dijkstra para encontrar el camino más corto. Esto supone que no siempre encontrará el camino óptimo, o que no todos los algoritmos se adaptan a todos los laberintos (con algunos tipos de laberintos, hay algoritmos que no pueden encontrar la solución).

# Descripción de la escena

La simulación empieza en un menú, con el que el jugador puede escoger qué mapa se va a utilizar, y qué algoritmo utilizará Caperucita (si el jugador quiere intentar resolver el laberinto por su cuenta, también puede).

Cuando el jugador decide empezar la simulación, encontrará un escenario un laberinto y a Caperucita en la entrada. Si ha escogido que sea la IA quien resuelva el laberinto, Caperucita empezará a moverse automáticamente. Además, a su paso, dejará un rastro que permite ver el camino recorrido hasta ahora.

# Descripción de la IA

En este caso, la IA se puede mover siguiendo cuatro comportamientos diferentes. Estos algoritmos son los más utilizados a la hora de resolver laberintos, y cada uno tiene unas ventajas y desventajas. Los algoritmos empleados son:
    
## Resolutor aleatorio. 
En cada intersección, decide qué dirección tomar, aleatoriamente.
- Ventajas: puede resolver, teóricamente, cualquier tipo de laberinto que tenga solución, ya que no tiene ningún tipo de limitación en el movimiento. 
- Desventajas: puede repetir el camino que ya ha recorrido, repetir infinitamente un camino, o no encontrar la salida nunca. No tiene memoria.

## Regla de la mano (izquierda o derecha). 
El personaje "pega la mano a una pared" (izquierda o derecha, según se le indique) y la sigue. 
- Ventajas: si el laberinto es simple (todas las paredes están interconectadas), encuentra siempre una solución, sin repetir caminos.
- Desventajas: si hay diferentes grupos de paredes, que no están conectados entre sí, puede no encontrar una solución. Si sigue un pilar, no podrá dejar de seguirlo (y no encontrará la solución). No tiene memoria.
## Algoritmo de Tremaux. 
En cada intersección, decide qué dirección tomar, siguiendo cuatro normas sencillas:

        - Cuando llegas a una intersección (y cuando sales de ella), marcas el pasillo por el que has llegado (o por el que sales).
        - Nunca salgas de una intersección por un pasillo que tenga dos marcas.
        - Si al llegar a una intersección, encuentras una marca, vuelve atrás.
        - Si al intentar volver atrás no puedes (porque el pasillo tiene dos marcas), ve por el pasillo con menos marcas.
 - Ventajas: es una mejora importante respecto al resolutor aleatorio. Tiene memoria; cuando marca un pasillo por segunda vez, no vuelve a cruzarlo. Es capaz de detectar laberintos que no tienen solución. Si el laberinto tiene solución, siempre la encuentra.
 - Desventajas: siempre que encuentra un camino con una marca, deshace sus pasos. Esto puede hacer tarde mucho más tiempo en encontrar la solución, si la hay.
 - 
## Algoritmo de "Pledge". 
Intenta avanzar en una dirección "favorita". Lleva un contador de giros, y se rige por estas normas:

        - Siempre te mueves en tu dirección favorita, hasta que te encuentras una pared que te lo impide.
        - En ese momento, empieza a seguir la pared (izquierda o derecha), y pon tu contador de giros a 0. 
        - Si sigues la pared izquierda, cada vez que haces un giro en sentido contrario a las agujas del reloj, suma uno a tu contador de giros; si giras en sentido horario, resta uno.
        - Si sigues la pared derecha, cada vez que haces un giro en sentido contrario a las agujas de reloj, resta uno a tu contador de giros; si giras en sentido horario, suma uno.
        - Si tu contador de giros está a 0 (no vale un múltiplo de 4) y estás mirando en tu dirección favorita, deja de seguir la pared.
- Ventajas: es una mejora importante respecto a la regla de la mano derecha. Si la salida está en una pared desconectada de la entrada, y en un "anillo exterior", este algoritmo puede encontrar la salida. 
- Desventajas: no puede encontrar la salida si la situación es la contraria: si el jugador empieza en un "anillo" exterior al de la salida. No tiene memoria.

# Pruebas realizadas y resultados obtenidos

![Algoritmo de Tremaux en un laberinto sin solución](https://user-images.githubusercontent.com/48750779/122249225-f2f73380-cec8-11eb-97dc-3bf7fe3a23b8.png)
# Bibliografía y recursos utilizados

- Unity 2018 Artificial Intelligence Cookbook, Second Edition [(Repositorio)]( https://github.com/PacktPublishing/Unity-2018-Artificial-Intelligence-Cookbook-Second-Edition)
- Unity Artificial Intelligence Programming, Fourth Edition [(Repositorio)]( https://github.com/PacktPublishing/Unity-Artificial-Intelligence-Programming-Fourth-Edition)
- Mazes For Programmers: Code Your Own Twisty Little Passages, First Edition [(Página del libro)](http://www.mazesforprogrammers.com/)
- [Think Labyrinth](http://www.astrolog.org/labyrnth/algrithm.htm), con una lista de algoritmos útiles para resolver laberintos, desde distintas perspectivas y con diferentes restricciones.

