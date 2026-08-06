#!/usr/bin/env python3
"""Generate Binimal ICO assets from original geometric artwork."""

from pathlib import Path
from PIL import Image, ImageDraw

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / "src" / "Binimal.App" / "Assets"
PREVIEW = ROOT / "assets" / "preview"
SIZES = (16, 20, 24, 32, 48, 64, 128, 256)
SCALE = 4
CANVAS = 256 * SCALE

NAVY = "#10233F"
DARK = "#111827"
BLUE = "#63B3FF"
GOLD = "#FFB454"


def scaled(points):
    return tuple(int(value * SCALE) for value in points)


def line(draw, points, fill, width, joint="curve"):
    draw.line([scaled(point) for point in points], fill=fill, width=width * SCALE, joint=joint)


def tray_icon(full):
    image = Image.new("RGBA", (CANVAS, CANVAS), (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)

    if full:
        paper = [(94, 102), (124, 118), (149, 102), (171, 126), (159, 183), (96, 183), (83, 126), (94, 102)]
        draw.polygon([scaled(point) for point in paper], fill=GOLD)
        line(draw, paper, NAVY, 12)

    paths = [
        [(55, 72), (201, 72)],
        [(102, 48), (154, 48)],
        [(72, 82), (184, 82), (171, 214), (85, 214), (72, 82)],
    ]
    for width, color in ((18, NAVY), (10, BLUE)):
        for path in paths:
            line(draw, path, color, width)

    return image


def app_icon():
    image = Image.new("RGBA", (CANVAS, CANVAS), (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    draw.rounded_rectangle(scaled((20, 20, 236, 236)), radius=48 * SCALE, fill=DARK)
    line(draw, [(72, 78), (184, 78)], BLUE, 16)
    line(draw, [(106, 60), (150, 60)], BLUE, 16)
    draw.polygon([scaled(point) for point in [(82, 92), (174, 92), (164, 196), (92, 196)]], fill=BLUE)
    line(draw, [(112, 116), (112, 172)], DARK, 10)
    line(draw, [(144, 116), (144, 172)], DARK, 10)
    return image


def save_icon(image, name):
    OUT.mkdir(parents=True, exist_ok=True)
    PREVIEW.mkdir(parents=True, exist_ok=True)
    master = image.resize((256, 256), Image.Resampling.LANCZOS)
    master.save(PREVIEW / f"{name}.png")
    master.save(OUT / f"{name}.ico", format="ICO", sizes=[(size, size) for size in SIZES])


def main():
    save_icon(app_icon(), "app")
    save_icon(tray_icon(False), "bin-empty")
    save_icon(tray_icon(True), "bin-full")


if __name__ == "__main__":
    main()
