#!/usr/bin/env python3
import argparse
import os
import re
from pathlib import Path


def extract_command_docs(commands_dir):
    entries = []

    def parse_string_literals(text):
        return re.findall(r'"([^"\\]*(?:\\.[^"\\]*)*)"', text)

    for root, _, files in os.walk(commands_dir):
        for fname in sorted(files):
            if not fname.endswith('.cs'):
                continue
            fpath = Path(root) / fname
            try:
                with open(fpath, 'r', encoding='utf-8') as f:
                    content = f.read()
            except Exception:
                continue

            # Find Aliases expression/initializer body content up to semicolon
            aliases = []
            m_alias = re.search(r'public\s+(?:override|static)\s+string\s*\[\s*\]\s*Aliases\s*(?:=>|=)\s*([\s\S]*?);', content)
            if m_alias:
                aliases = parse_string_literals(m_alias.group(1))

            # Find Description property or field
            description = None
            m_desc = re.search(r'public\s+(?:override|static)\s+string\s+Description\s*(?:=>|=)\s*([^;]+);', content)
            if m_desc:
                # take the first string literal
                lits = parse_string_literals(m_desc.group(1))
                if lits:
                    description = lits[0]

            if not aliases or description is None:
                continue

            for a in aliases:
                a_norm = re.sub(r"\s+", " ", a.strip())
                d_norm = re.sub(r"\s+", " ", description.strip())
                entries.append(f"{a_norm} = {d_norm}")

    # Stable sort and deduplicate identical lines
    seen = set()
    uniq = []
    for e in sorted(entries):
        if e not in seen:
            seen.add(e)
            uniq.append(e)
    return uniq


def replace_code_block(readme_path, lines):
    # Build the entire README from a predefined header and the generated commands block.
    header_lines = [
        "# MoreCommands",
        "",
        "Adds console commands for White Knuckle.",
        "",
        "### Console commands added:",
        "",
        "```",
    ]
    footer_lines = [
        "```",
        "",  # ensure a single trailing newline at EOF
    ]

    new_content = "\n".join(header_lines + list(lines) + footer_lines)

    try:
        with open(readme_path, 'r', encoding='utf-8') as f:
            current = f.read()
    except FileNotFoundError:
        current = None

    if current != new_content:
        with open(readme_path, 'w', encoding='utf-8') as f:
            f.write(new_content)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--commands-dir', required=True)
    parser.add_argument('--readme', required=True)
    args = parser.parse_args()

    commands_dir = Path(args.commands_dir)
    readme_path = Path(args.readme)

    entries = extract_command_docs(commands_dir)
    replace_code_block(readme_path, entries)


if __name__ == '__main__':
    main()


