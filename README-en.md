# Roger

My neural network, which I improve every day.

Copyright (c) 2025 Axolotl512, death_script. License: MIT.

AI suitable for embedding in robots and games.

## Versions and Architecture

- **Quecto Roger 1 and below** — LP architecture, .NET 8.0.
- **Quecto Roger 2 and above** — MLP architecture, .NET 9.0.
- **Yocto Roger 2.1** — MLP architecture, .NET 9.0.

## Running Yocto Roger 2.1 on Windows

1. Install .NET 9.0.
2. Navigate to the following path: `AI_Roger/AI_Roger/bin/Release/net9.0`
3. Run `AI_Roger.exe`.
4. > Or simply download the release binary
5. Edit the knowledge file (knowledge.know) or create your own (more details in the wiki).

### Training Parameters

- **LR (Learning Rate)** — the lower the value, the more accurate the training.
- **Passes** — the higher the value, the better the network learns.
- **Knowledge File** — the file on which the neural network is trained.
- **DropOut** (only in `AI_Roger.exe`) — protection against overfitting.
Recommended values: **5%** or **10%**.
When using DropOut, a lower LR and more passes are usually needed.

## Running on Linux ARM (aarch64, AI_Roger)

1. In the folder with the files, run:
`./AI_Roger`
2. Edit the knowledge file `knowledge.know` (more details in the wiki).

### Training Parameters

- **LR (Learning Rate)** — the lower the value, the more accurate the training.
- **Passes** — the higher the value, the better the network learns.
- **Knowledge File** — the file on which the neural network is trained.

> ⚠️ For the ARM version of Yocto Roger 2.1 / 3.0, installing .NET 9 is not required.

## Reminder
All announcements, news, and other information about the project will be posted in the Telegram channel: [Link to our Telegram channel](https://t.me/Axolotl1024).
And also our website: [Website](https://emotioncorp.site) [Temporarily down]

Sincerely, the Emotion team ;)