#!/usr/bin/env python3
import argparse
import os
import re
from pathlib import Path

from dataclasses import dataclass
from typing import List

@dataclass
class CommandData:
    aliases_str: str
    description: str
    cmd_type: str
    cheats_only: bool


all_aliases = ['godmode', 'deathgoo-stop', 'fullbright', 'infinitestamina', 'notarget', 'noclip', 'deathgoo-heiht']

def extract_commands(commands_dir):
    global all_aliases
    command_files = []
    for root, _, files in os.walk(commands_dir):
        for fname in sorted(files):
            if not fname.endswith('.cs'):
                continue
            command_files.append(Path(root) / fname)
    
    commands = []
    for fpath in command_files:
        with open(fpath, 'r', encoding='utf-8') as f:
            cmd_type = "Oneshot"
            for line in f.readlines():
                if line.count("TogglableCommandBase"):
                    cmd_type = "Togglable"
                if line.count("public override string[] Aliases"):
                    aliases_str = line[41:-3].replace('"', '`')
                    all_aliases += aliases_str.replace('`', '').split(', ')
                if line.count("public override string Description =>"):
                    description = line[43:-3].replace('\\n', ', ')
                if line.count("public override bool CheatsOnly"):
                    cheats_only = line.count("true") > 0
                
            commands.append(CommandData(aliases_str, description, cmd_type, cheats_only))
    
    all_aliases = sorted(all_aliases, key=len, reverse=True)
    return commands


def generate_markdown_table(commands: List[CommandData]) -> str:
    global all_aliases
    headers = ["Command", "Description", "Type", "Enables cheats"]
    
    processed_rows = []
    pattern = r"\b(" + "|".join(re.escape(k) for k in all_aliases) + r")\b"

    for cmd in commands:
        cheats_str = "`+`" if cmd.cheats_only else "`-`"
        
        
        processed_rows.append([
            cmd.aliases_str,
            re.sub(pattern, r"`\1`", cmd.description),
            cmd.cmd_type,
            cheats_str
        ])

    col_widths = [len(h) for h in headers]
    for row in processed_rows:
        for i, cell in enumerate(row):
            if len(cell) > col_widths[i]:
                col_widths[i] = len(cell)

    def make_row(cells, align="left"):
        row_str = "|"
        for i, cell in enumerate(cells):
            padding = " " * (col_widths[i] - len(cell))
            row_str += f" {cell}{padding} |"
        return row_str

    separator_parts = []
    for i, w in enumerate(col_widths):
        if i < 2:
            separator_parts.append("-" * w)
        else:
            dash_len = max(3, w) 
            separator_parts.append(":" + "-" * (dash_len - 2) + ":")

    lines = [make_row(headers)]
    
    sep_line = "|"
    for i, sep in enumerate(separator_parts):
        target_len = max(len(sep), col_widths[i])
        final_sep = sep
        if len(sep) < target_len:
             if i >= 2:
                 final_sep = ":" + "-" * (target_len - 2) + ":"
             else:
                 final_sep = "-" * target_len
        sep_line += f" {final_sep} |"
    lines.append(sep_line)

    for row in processed_rows:
        lines.append(make_row(row))

    return "\n".join(lines)



def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--commands-dir', required=True)
    parser.add_argument('--output', required=True)
    args = parser.parse_args()

    commands_dir = Path(args.commands_dir)
    output_path = Path(args.output)

    commands = extract_commands(commands_dir)
    output_path.write_text(generate_markdown_table(commands))

if __name__ == '__main__':
    main()

