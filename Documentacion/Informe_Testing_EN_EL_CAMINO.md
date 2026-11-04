# Informe de testing — En el Camino

**Materia:** Motores de Desarrollo 1 — Parcial 2 
**Proyecto:** En el Camino  
**Motor:** Unity 6000.3.5f2  
**Período de pruebas:** 20/10/2026 al 04/11/2026

## Objetivo de las pruebas

Fui probando el juego durante su desarrollo para revisar que las misiones se pudieran completar, que los controles respondieran bien y que la persecución final funcionara. También revisé la cámara, los sonidos, las animaciones, el tráfico y las pantallas de victoria y derrota.

Las pruebas se hicieron a medida que avanzaba el proyecto. Los problemas encontrados en las versiones anteriores se fueron corrigiendo y volviendo a probar. Al cerrar el desarrollo, realicé las pruebas finales del juego. La dificultad que quedó sin resolver corresponde a la versión para navegador.

## Resumen de lo probado

| Parte del juego | Qué revisé | Estado |
| --- | --- | --- |
| Llegada a la estación | El auto se queda sin combustible y comienza el tiempo de la misión. | Probado |
| Personaje y taller | Caminar, correr, subir al techo, bajar al taller e interactuar con los objetos. | Probado |
| Generador y surtidor | Encender el generador, conectar la electricidad, abrir el portón, recoger y llenar el bidón. | Probado |
| Regreso al auto | Cargar combustible, volver a conducir y salir a la ruta antes de que termine el tiempo. | Probado |
| Persecución | Aparición del patrullero, sirena, seguimiento del jugador y desafío de los 200 metros en 60 segundos. | Probado, con observaciones |
| Tráfico y choques | Circulación en ambos sentidos, frenadas, contactos y daño del vehículo. | Probado, con observaciones |
| Presentación | Cámara, retrovisor, animaciones, iluminación, partículas, música y efectos de sonido. | Probado |
| Menús y resultados | Pausa, controles, reinicio, victoria y Game Over. | Probado |

## Registro de pruebas y correcciones

### Del 20 al 23/10/2026

Comencé las revisiones de las misiones y los controles. En los videos del **23/10** detecté dificultades con el tráfico, la respuesta del patrullero y la pantalla de resultado.

A partir de esas pruebas ajusté la circulación de los autos, el comportamiento del vehículo después de un choque, el reloj inicial de dos minutos y la forma en que se detiene el juego al ganar. También revisé la legibilidad de los mensajes de pantalla.

### 24/10/2026

Trabajé sobre la cámara, el retrovisor, el sonido y el funcionamiento de la patrulla. Corregí situaciones en las que la policía se quedaba detenida o perdía el camino hacia el jugador.

También ajusté el comienzo de la sirena y de la música de persecución para evitar una pausa al activarlas, y revisé los efectos de humo, polvo y chispas.

Se realizaron comprobaciones en Unity y pruebas parciales del recorrido. Los problemas de esta fecha corresponden a versiones anteriores del juego.

### 25/10/2026

Revisé las animaciones del personaje, la cámara durante los movimientos y la incorporación del patrullero a la ruta. La patrulla debía entrar en el mismo sentido del tránsito y después comenzar a perseguir al jugador.

Además ajusté el polvo al circular sobre pasto, el humo del escape y la reacción del tráfico cuando los vehículos se encuentran o chocan. Estas correcciones surgieron de las pruebas y videos de ese día.

### 26/10/2026

Hice las últimas revisiones registradas de la persecución. Los problemas principales eran la velocidad de la patrulla, el seguimiento al salir al pasto y la circulación entre otros vehículos.

Después de los ajustes, las simulaciones de persecución verificaron que la patrulla podía incorporarse, seguir al jugador, acompañar los cambios entre ruta y pasto y recuperar distancia. También se revisaron los contactos con el tráfico.

Estas simulaciones sirvieron para corregir comportamientos, pero no eran por sí solas una partida completa. Las pruebas jugables finales se realizaron posteriormente.

### 03/11/2026 — Intento de publicación en itch.io

Intenté preparar una versión **WebGL** para que el juego se pudiera ejecutar directamente desde itch.io, sin descargarlo. La compilación no funcionó correctamente: al iniciar el juego, averigue el error y es que el assets **Vehicle Physics Pro** (`VehiclePhysics.dll`) intentaba leer un recurso interno de la DLL que no se encontraba en WebGL.

## Observaciones finales

Durante el desarrollo también aparecieron dos problemas ocasionales: el patrullero puede trabarse con algunos obstáculos mientras intenta seguir al jugador y los autos de tráfico pueden superponerse después de choques fuertes. Son comportamientos observados que conviene dejar registrados, aunque las pruebas del juego ya se hayan realizado.

El recorrido principal, las interacciones, la conducción, la persecución y los resultados se probaron durante el desarrollo. La **limitación pendiente es la publicación de una versión WebGL funcional**; no debe confundirse con un fallo general de la versión para Windows.

**Descarga para Windows:** [Google Drive](https://drive.google.com/file/d/1d6g3U620LQHoLpT9G-QZ4_GeA8l6nMaN/view?usp=sharing).
