// dangerfile.ts
import { fail, warn, message } from "danger";
import fs from "fs";

const resultsFile = "analysis-results.txt";

if (fs.existsSync(resultsFile)) {
    const lines = fs.readFileSync(resultsFile, "utf8").split("\n");

    for (const line of lines) {
        const match = line.match(/(.+)\((\d+),(\d+)\): (warning|error) (\w+): (.+)/);
        if (match) {
            const [, file, lineNum, col, level, code, msg] = match;
            const emoji = level === "error" ? "❌" : "⚠️";
            const title = `${emoji} ${code}: ${msg}`;
            fail(title, file, parseInt(lineNum));
        }
    }
}
