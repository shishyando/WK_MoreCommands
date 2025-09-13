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

            # Find Aliases expression body content up to semicolon
            aliases = []
            m_alias = re.search(r'public\s+static\s+string\s*\[\s*\]\s*Aliases\s*=>\s*([\s\S]*?);', content)
            if not m_alias:
                # try initializer style
                m_alias = re.search(r'public\s+static\s+string\s*\[\s*\]\s*Aliases\s*=\s*([\s\S]*?);', content)
            if m_alias:
                aliases = parse_string_literals(m_alias.group(1))

            # Find Description property or field
            description = None
            m_desc = re.search(r'public\s+static\s+string\s+Description\s*=>\s*([^;]+);', content)
            if not m_desc:
                m_desc = re.search(r'public\s+static\s+string\s+Description\s*=\s*([^;]+);', content)
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
    with open(readme_path, 'r', encoding='utf-8') as f:
        content = f.read()

    # Replace content between the first fenced code block after the heading "### Console commands added:".
    # It should look like:
    # ### Console commands added:
    # ```
    # ...
    # ```
    heading_pattern = re.compile(r"(###\s+Console\s+commands\s+added:\s*\n)```[\s\S]*?```", re.MULTILINE)

    new_block = "\\g<1>```\n{body}\n```\n".format(body="\n".join(lines))

    if heading_pattern.search(content):
        updated = heading_pattern.sub(new_block, content, count=1)
    else:
        # Append a new section at the end if not found
        sep = "\n" if content.endswith("\n") else "\n\n"
        updated = content + f"{sep}### Console commands added:\n```\n{os.linesep.join(lines)}\n```\n"

    if updated != content:
        with open(readme_path, 'w', encoding='utf-8') as f:
            f.write(updated)


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


