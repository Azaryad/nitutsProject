# Visual Paradigm Import Package

Visual Paradigm's **Import Visual UML (`.umlx`)** dialog does not import PlantUML source directly. The files here give you two practical ways to bring the implementation diagrams into Visual Paradigm.

## Ready-to-open project

Open this project directly in Visual Paradigm:

```text
docs/design/visual-paradigm-import/implementation-uml-visuals.vpp
```

It is a copy of `docs/VPPARTA.vpp` with 12 added diagram pages named `Implementation UML - ...`.

The added pages are rendered PNG visuals linked from:

```text
C:\Users\Public\VPImageImport\rendered-images
```

Keep that folder if you want the image-backed pages to keep displaying. The same PNGs are also stored in:

```text
docs/design/visual-paradigm-import/rendered-images
```

## Single Trip state-machine project

Open this project directly in Visual Paradigm:

```text
docs/design/visual-paradigm-import/state_trip.vpp
```

It is a copy of `docs/VPPARTA.vpp` with one added diagram page named `Trip - State Machine`, rendered from:

```text
docs/design/visual-paradigm-import/plantuml-diagrams/state_trip.puml
```

The added page is linked to:

```text
C:\Users\Public\VPStateTripImage\rendered-images\state_trip.png
```

Keep that folder if you want the image-backed page to keep displaying. The same PNG is also stored in:

```text
docs/design/visual-paradigm-import/rendered-images/state_trip.png
```

## Source files

The original all-in-one PlantUML file is:

```text
docs/design/implementation-uml.puml
```

For tools/plugins that require one diagram per file, use:

```text
docs/design/visual-paradigm-import/plantuml-diagrams
```

## Optional editable PlantUML import

The included plugin zip can be installed through **Help > Install Plugin > Install from a zip of plugin**:

```text
docs/design/visual-paradigm-import/plugin-plantuml-vp-v1.0.0.zip
```

That plugin can import the `plantuml-diagrams` folder, but the conversion is parser-dependent and may show warnings for nonstandard overview diagrams. The ready-to-open `.vpp` above is the stable visual import.
