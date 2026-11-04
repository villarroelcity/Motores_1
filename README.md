# En el Camino

Motores de Desarrollo 1 – Unity - Parcial 2. (vertical slice)

Juego en tercera persona: el personaje viene escapando de una persecución policial por no parar en un control. Logra evadir a la patrulla y sigue su camino, pero el auto se queda sin combustible en medio de la ruta, junto a una estación de servicio abandonada. Baja del auto y busca la manera de darle electricidad a la estación para encender el surtidor y cargar un bidón. Tiene una ventana de dos minutos antes de que la policía lo encuentre y lo detenga.

- Versión para Windows (descarga desde Drive): https://drive.google.com/file/d/1d6g3U620LQHoLpT9G-QZ4_GeA8l6nMaN/view?usp=sharing
- Video de presentación: https://www.youtube.com/watch?v=H8hDDF1x3-o
- Motor: Unity 6000.3.5f2 (URP)

## Objetivo

1. Bajar del auto, subir al techo y entrar al taller.
2. Encender el generador, conectar el tablero eléctrico y abrir el portón.
3. Recoger el bidón, cargarlo en el surtidor y abastecer el auto.
4. Conducir hasta la salida antes de que termine el tiempo.
5. En la ruta, superar la emboscada y sacarle 200 m de ventaja a la patrulla antes de 60 segundos.

## Controles

| Tecla | Acción |
| --- | --- |
| `WASD` / flechas | Mover al personaje o conducir |
| `Shift` | Correr |
| `E` | Interactuar con el objeto indicado |
| `S` / flecha abajo | Frenar y luego retroceder (en el auto) |
| `Espacio` | Freno de mano (en el auto) |
| `Esc` | Pausa |
| `R` | Reiniciar (desde pausa o resultado) |

## Sistemas integrados

Animator, audio (música, ambiente y efectos con mixer), UI, Cinemachine, iluminación, IA de patrulla con NavMesh, sistema de partículas, tráfico, daño, triggers/eventos y desafío final de persecución.

## Estructura del proyecto

```
Assets/
  Proyecto/          contenido propio
    Scenes/          EnElCamino.unity (única escena)
    Scripts/         código de gameplay
    PreparedGameplay/ prefabs, animaciones, efectos, UI, navegación
    Materials/ Audio/ Models/ Shaders/ Editor/
  ThirdParty/        packs externos con licencia libre (Kenney, FastMesh, etc.)
Documentacion/       GDD e informe de testing
```

## Abrir el proyecto

1. Instalar Unity **6000.3.5f2** y abrir la carpeta desde Unity Hub.
2. Importar los paquetes comerciales que no están en el repositorio por licencia: **Vehicle Physics Pro** y **Nature Starter Kit 2** (carpetas `Assets/Vehicle Physics Pro` y `Assets/NatureStarterKit2`). _(pendiente: link de Drive con los paquetes si se permite compartirlos)_
3. Abrir `Assets/Proyecto/Scenes/EnElCamino.unity` y esperar la importación antes de dar Play.

## Documentación

- [GDD](Documentacion/GDD_EN_EL_CAMINO.md)
- [Informe de testing](Documentacion/Informe_Testing_EN_EL_CAMINO.md)

## Bugs conocidos

- El patrullero a veces se traba con el entorno tratando de seguir al jugador.
- El tráfico puede superponerse en choques fuertes.

## Créditos

- Vehicle Physics Pro (Edy).
- Nature Starter Kit 2.
- Kenney.
- Poly Haven.
- FastMesh, Fuel Pump 3D.
- Old Sign Roads y Road Props for Games.

Recursos externos: ver `Assets/ThirdParty/ExternalModels/ASSET_LICENSES.txt` y `Assets/ThirdParty/Kenney/README.md`.
