# En el Camino

## Documento de diseño del juego (GDD)

**Materia:** Motores de Desarrollo 1  
**Trabajo:** Segundo Parcial  
**Autor:** Ezequiel Godoy  
**Motor:** Unity 6  
**Plataforma:** PC (Windows)  
**Género:** Aventura de acción, exploración y conducción en tercera persona

## 1. Idea del juego

*En el Camino* es un juego corto en tercera persona que combina exploración, resolución de pequeños problemas y conducción. Quise que una situación que parece sencilla —quedarse sin nafta en la ruta— se convierta en una carrera contra el tiempo.

El jugador tiene que recorrer una estación de servicio abandonada, recuperar la electricidad, conseguir combustible y volver al auto. Cuando parece que pudo continuar su viaje, comienza una persecución policial que cambia por completo el ritmo de la partida.

El juego está pensado como una experiencia breve, con un principio, un desafío central y un final definido.

## 2. Historia

El protagonista viaja por una ruta después de escapar de la policía por no detenerse en un control. Logra perder a la patrulla, pero poco después el auto se queda sin combustible y termina detenido junto a una estación de servicio abandonada.

Tiene aproximadamente dos minutos para resolver el problema y volver a la ruta antes de que lo encuentren. En ese momento no hay un patrullero siguiéndolo de cerca; la presión viene del poco tiempo que tiene para escapar de la zona.

Al explorar la estación descubre que no hay electricidad y que el taller está cerrado. Para entrar debe subir al techo y bajar por una abertura. Dentro encuentra un generador que todavía tiene algo de combustible, pero solo puede funcionar durante treinta segundos.

Con esa energía debe encender la instalación eléctrica, abrir el portón, conseguir un bidón y llenarlo en el surtidor. Después tiene que llevarlo hasta el auto, cargar combustible y salir a la ruta.

Sin embargo, el peligro no terminó: un patrullero lo espera más adelante. Cuando lo descubre, enciende la sirena y comienza a perseguirlo. El jugador debe escapar entre el tráfico y conseguir suficiente distancia para ganar.

## 3. Objetivo del jugador

El objetivo principal es **conseguir combustible, regresar a la ruta y escapar de la policía**.

La partida tiene dos grandes momentos:

- **En la estación:** explorar, encontrar cómo entrar al taller, activar la electricidad y cargar nafta antes de que se termine el tiempo.
- **En la ruta:** conducir, evitar choques y conseguir 200 metros de ventaja sobre el patrullero en un máximo de 60 segundos.

El jugador necesita prestar atención tanto a los mensajes de objetivos como a los relojes. Una acción realizada en el orden incorrecto puede impedir avanzar.

## 4. Cómo se juega

### Llegada a la estación

La partida empieza con el auto en movimiento. El motor pierde fuerza por falta de combustible y el vehículo termina deteniéndose. A partir de ese momento el jugador baja del auto y comienza a buscar una solución.

El tiempo de la primera etapa es de **dos minutos**. No alcanza con encontrar combustible: hay que volver a poner el auto en marcha y llegar a la salida de la estación antes de que termine la cuenta.

### Entrada al taller

La puerta del taller no se puede utilizar al principio. El jugador debe encontrar el acceso por el techo y descender al interior. Esto obliga a observar el escenario y no simplemente caminar hasta el surtidor.

### Generador y combustible

Dentro del taller se encuentra el generador. Al encenderlo empieza una cuenta de **30 segundos**. Durante ese tiempo hay que conectar la electricidad desde el tablero, abrir el portón, recoger el bidón vacío y llenarlo en el surtidor.

Si el generador se apaga antes de terminar de llenar el bidón, se pierde la partida. En cambio, si el bidón ya está lleno, todavía es posible llevarlo hasta el auto, siempre que no se haya terminado el tiempo general de la estación.

Los dos relojes funcionan por separado: encender el generador no devuelve los dos minutos iniciales.

### Regreso al auto

Con el bidón lleno, el jugador vuelve al vehículo, carga el combustible y arranca. Después debe conducir hasta la salida de la estación. Una vez en la ruta, continúa la partida: todavía falta escapar de la policía.

### Persecución

Más adelante aparece un patrullero que comienza a seguir al jugador. En esta parte el desafío deja de ser buscar objetos y pasa a ser conducir bajo presión.

Hay **60 segundos para conseguir 200 metros de ventaja**. El tráfico circula en ambos sentidos, por lo que hace falta esquivar vehículos, elegir por dónde pasar y evitar perder velocidad en los choques. También es posible salir del asfalto, aunque el auto se comporta de otra manera sobre el pasto.

## 5. Reglas y resultados

| Situación | Resultado |
| --- | --- |
| Salir de la estación antes de que terminen los dos minutos | Se supera la primera parte y continúa la historia. |
| No salir de la estación a tiempo | Fin de la partida. |
| Dejar que se apague el generador sin haber llenado el bidón | Fin de la partida. |
| Conseguir 200 metros de ventaja durante la persecución | Victoria. |
| Agotar los 60 segundos sin lograr la ventaja | Fin de la partida. |
| Ser alcanzado por la patrulla, destruir el auto o sufrir una caída peligrosa | Fin de la partida. |

Al ganar se muestra el resultado final. Al perder aparece la pantalla de **GAME OVER** y se puede reiniciar para volver a intentarlo.

## 6. Controles

| Tecla | Acción |
| --- | --- |
| **WASD** o **flechas** | Mover al personaje o conducir el auto. |
| **Shift** | Correr cuando el personaje está a pie. |
| **E** | Interactuar con objetos cercanos. |
| **S** o **flecha abajo** | Frenar y luego retroceder cuando se conduce. |
| **Espacio** | Freno de mano del auto. |
| **Esc** | Abrir o cerrar la pausa. |
| **R** | Reiniciar desde la pausa o la pantalla de resultado. |

Los controles cambian según si el jugador está caminando o conduciendo, pero se mantienen simples para que el desafío esté en lo que sucede dentro del juego y no en memorizar muchas teclas.

## 7. Escenario y estilo visual

Elegí una ruta rural y una estación de servicio abandonada porque permiten contar la situación sin necesitar un mapa demasiado grande. El escenario incluye el camino, la estación, el taller, el techo por el que se accede, el generador, el tablero eléctrico, el portón y el surtidor.

El estilo visual es sencillo, con modelos de apariencia *low-poly*, vegetación y una iluminación que cambia gradualmente mientras transcurre la partida. El paso de la tarde a la noche ayuda a transmitir urgencia, aunque el motivo principal del límite de tiempo es que la policía puede encontrar al protagonista.

La cámara sigue al personaje cuando camina y al vehículo cuando conduce. Durante la conducción también hay un retrovisor para observar lo que ocurre detrás.

## 8. Sonido y ambiente

El sonido ayuda a entender lo que está ocurriendo: el motor que se apaga, los pasos, el generador, el portón, el surtidor y los vehículos de la ruta tienen sonidos relacionados con sus acciones.

Al comienzo predomina el ambiente de la estación y la exploración. Cuando aparece el patrullero se escuchan la sirena y una música más intensa, que anuncian el cambio hacia la persecución.

También se utilizan efectos visuales sencillos, como humo del generador y del escape, polvo al conducir sobre el pasto y chispas en algunos impactos.

## 9. Información que recibe el jugador

Durante la partida aparecen mensajes cortos que indican qué hay que hacer a continuación. La pantalla también muestra el tiempo disponible en la estación y, cuando corresponde, el tiempo restante del generador.

Durante la persecución la información más importante es la distancia de ventaja sobre el patrullero y el tiempo que queda para escapar. Si el auto recibe golpes, se muestra un indicador de daño.

La intención es que el jugador pueda entender rápidamente su siguiente objetivo sin tener que leer instrucciones largas mientras corre o conduce.

## 10. Qué incluye esta versión

Esta versión del juego reúne en una sola experiencia la exploración a pie, la interacción con objetos, la conducción, el tráfico, los daños del vehículo, la aparición del patrullero y las condiciones de victoria o derrota.

Para desarrollarla utilicé Unity 6 y distintos recursos gráficos, de sonido y de conducción. Entre ellos se encuentran Vehicle Physics Pro y Nature Starter Kit 2, además de otros modelos y materiales incorporados al escenario.

El proyecto está planteado como un **vertical slice** para el segundo parcial: una parte jugable que muestra los sistemas principales y cómo se conectan entre sí, no un juego de mundo abierto ni una historia con varios niveles.

## 11. Estado de la entrega

El proyecto cuenta con una versión jugable para Windows y una página de descarga. En la documentación del repositorio también hay un informe de pruebas, donde se registran los problemas encontrados y los casos que deben revisarse.

Entre los problemas conocidos figuran situaciones en las que el patrullero puede trabarse con el entorno y choques fuertes en los que los autos del tráfico pueden superponerse. Esos problemas no cambian el objetivo ni las reglas del juego, pero forman parte de lo que todavía se puede mejorar.

**Versión de descarga:** [En el Camino en itch.io](https://ezequiel-godoy.itch.io/en-el-camino-parcial-2)  
**Informe de pruebas:** [Informe de testing](Informe_Testing_EN_EL_CAMINO.md)
