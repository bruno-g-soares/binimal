#!/usr/bin/env python3
# pyright: reportMissingImports=false
"""Generate neutral Binimal ICO assets from MIT-licensed Fluent UI SVGs."""

from io import BytesIO
from pathlib import Path

import cairosvg
from PIL import Image, ImageFilter

ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / "assets" / "source" / "fluent-ui-system-icons"
OUT = ROOT / "src" / "Binimal.App" / "Assets"
PREVIEW = ROOT / "assets" / "preview"
SIZES = (16, 20, 24, 32, 48, 64, 128, 256)
MASTER_SIZE = 256

# A light glyph with a charcoal keyline remains visible on both light and dark
# taskbars without adding a coloured accent or depending on Windows theme APIs.
GLYPH = (244, 244, 244, 255)
KEYLINE = (24, 24, 24, 255)
KEYLINE_RADIUS = 12


def svg_mask(filename: str) -> Image.Image:
    svg = (SOURCE / filename).read_bytes()
    png = cairosvg.svg2png(
        bytestring=svg,
        output_width=MASTER_SIZE,
        output_height=MASTER_SIZE,
    )
    return Image.open(BytesIO(png)).convert("RGBA").getchannel("A")


def keyed_glyph(filename: str) -> Image.Image:
    mask = svg_mask(filename)
    keyline_mask = mask.filter(ImageFilter.MaxFilter(KEYLINE_RADIUS * 2 + 1))

    image = Image.new("RGBA", (MASTER_SIZE, MASTER_SIZE), (0, 0, 0, 0))
    image.paste(KEYLINE, mask=keyline_mask)
    image.paste(GLYPH, mask=mask)
    return image


def save_icon(image: Image.Image, name: str) -> None:
    OUT.mkdir(parents=True, exist_ok=True)
    PREVIEW.mkdir(parents=True, exist_ok=True)
    image.save(PREVIEW / f"{name}.png")
    image.save(OUT / f"{name}.ico", format="ICO", sizes=[(size, size) for size in SIZES])


def main() -> None:
    empty = keyed_glyph("ic_fluent_delete_16_regular.svg")
    full = keyed_glyph("ic_fluent_delete_16_filled.svg")

    save_icon(full, "app")
    save_icon(empty, "bin-empty")
    save_icon(full, "bin-full")


if __name__ == "__main__":
    main()
