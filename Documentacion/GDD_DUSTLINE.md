# GDD — DUSTLINE

## Proyecto

**DUSTLINE**  
Motores de Desarrollo 1 — Unity  
Versión del documento: Parcial 1 / Prototipo inicial

**Integrantes:** [COMPLETAR]

## Género

Aventura / exploración en tercera persona, con conducción planificada para la siguiente etapa.

## High Concept

DUSTLINE es una aventura de exploración y conducción en tercera persona donde un mensajero mecánico recupera vehículos abandonados para reconstruir las rutas entre asentamientos aislados.

## Objetivo principal

Recuperar vehículos e infraestructura para restablecer las conexiones entre zonas aisladas.

## Mecánica central

**Recuperación de vehículos:** explorar una instalación, identificar qué necesita un vehículo y realizar las acciones necesarias para dejarlo operativo.

## Mecánicas secundarias previstas

### Implementadas en Parcial 1

- Movimiento en tercera persona.
- Cámara de seguimiento.
- Exploración de un greybox.
- Interacción mediante Raycast.
- Activación del generador.
- Instanciación de batería.
- Recogida de batería.
- Instalación en vehículo.
- Apertura de portón.
- Físicas, colisiones y trigger final.

### Planificadas para etapas posteriores

- Entrada y salida de vehículos.
- Conducción con sistema de física vehicular.
- Recorrido exterior entre zonas.
- Nuevos puntos de reparación.
- Diferentes vehículos/terrenos si el alcance lo permite.

## Gameplay actual

1. El jugador aparece dentro del taller.
2. Puede explorar a pie usando WASD y cámara con mouse.
3. Al inspeccionar el vehículo descubre que necesita una batería.
4. Activa el generador.
5. El generador instancia una batería física.
6. El jugador recoge la batería.
7. Instala la batería en el vehículo.
8. El portón se abre.
9. El jugador cruza la zona de salida y completa el prototipo.

## Core Gameplay Loop completo previsto

`explorar → diagnosticar → recuperar/reparar → conducir → llegar a nueva zona → repetir`

## Diseño preliminar del nivel

```text
┌──────────────────────────────────────┐
│               SALIDA                 │
│            [FinishTrigger]           │
│                   │                  │
│              [PORTÓN]                │
│                   │                  │
│          VEHÍCULO                    │
│           [AUTO]                     │
│                                      │
│ GENERADOR          ZONA DE TALLER    │
│   [G]                   [CAJAS]      │
│     \                                │
│      \ batería                       │
│                                      │
│              PLAYER                  │
│                [P]                   │
└──────────────────────────────────────┘
```

## Estado actual de desarrollo

### Funcionalidades implementadas

- Proyecto configurado en Unity 6000.3.5f2.
- URP.
- New Input System.
- `CharacterController`.
- Cámara tercera persona.
- Escena `Dustline_Prototype`.
- Greybox navegable.
- `IInteractable`.
- Raycast.
- GeneratorInteractable.
- `Battery.prefab`.
- `Instantiate`.
- Rigidbody/Collider.
- `OnCollisionEnter`.
- Recogida y `Destroy` controlado.
- VehicleInteractable.
- GarageDoor.
- `OnTriggerEnter` de finalización.

### Pendiente de validación manual en Unity

- Confirmar que no existan errores de compilación/importación.
- Ajustar posiciones o cámara si hiciera falta.
- Completar testing real.
- Generar build.
- Publicar en itch.io.

### Funcionalidades futuras

- Conducción.
- Integración con NWH Vehicle Physics 2 en la etapa correspondiente.
- Ruta exterior.
- Segunda zona.
- Presentación visual y sonora más avanzada.

## Alcance

El Parcial 1 se concentra en demostrar los sistemas técnicos de las clases 1 a 7 mediante un prototipo pequeño y estable. La conducción y el vertical slice se dejan para el desarrollo posterior.
