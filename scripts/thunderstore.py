import argparse, os, zipfile

p = argparse.ArgumentParser()
p.add_argument("--out", required=True)
p.add_argument("--readme", required=True)
p.add_argument("--changelog", required=True)
p.add_argument("--manifest", required=True)
p.add_argument("--icon", required=True)
p.add_argument("--dll", required=True)
args = p.parse_args()

if os.path.exists(args.out):
    os.remove(args.out)

with zipfile.ZipFile(args.out, "w", compression=zipfile.ZIP_DEFLATED) as z:
    z.write(args.readme,    "README.md")
    z.write(args.changelog, "CHANGELOG.md")
    z.write(args.manifest,  "manifest.json")
    z.write(args.icon,      "icon.png")
    z.write(args.dll,       os.path.basename(args.dll))

print(f"Thunderstore ZIP ready: {args.out}")
