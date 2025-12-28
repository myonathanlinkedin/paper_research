# RuntimeErrorSage: Intelligent Runtime Error Analysis and Remediation

RuntimeErrorSage is a .NET middleware system that enhances application reliability through intelligent runtime error analysis and remediation using local Large Language Models (LLMs). This implementation is based on the research paper described in `paper/main.tex`.

## Quick Start

- 📖 [Documentation](docs/README.md) - Complete documentation
- 🚀 [Getting Started](docs/getting-started/overview.md) - Introduction and overview
- 🏗️ [Architecture](docs/architecture/overview.md) - System architecture
- 📋 [Installation](docs/installation/setup.md) - Setup guide
- 🔬 [Research Contributions](docs/research/contributions.md) - Key research contributions
- 📊 [Evaluation Results](docs/evaluation/results.md) - Performance metrics

## Overview

RuntimeErrorSage introduces a novel approach to runtime error handling by combining:
- Graph-based context analysis
- Local LLM-powered error understanding
- Automated remediation strategies
- Model Context Protocol (MCP) for distributed context management

## Key Features

1. **Intelligent Error Analysis** - Context-aware error tracking with natural language explanations
2. **Automated Remediation** - Real-time error interception with intelligent suggestions
3. **Privacy-Preserving** - Local LLM inference ensures data privacy
4. **Easy Integration** - Simple middleware integration for ASP.NET Core applications

## Quick Setup

```bash
# Clone the repository
git clone https://github.com/yourusername/RuntimeErrorSage.git
cd RuntimeErrorSage

# Install dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test
```

For detailed setup instructions, see the [Installation Guide](docs/installation/setup.md).

## Documentation

> **Note**: The documentation is stored in a private repository as a Git submodule. To access the documentation, you need:
> 1. Access to the private documentation repository
> 2. Initialize and update the submodule after cloning:
>    ```bash
>    git submodule init
>    git submodule update
>    ```
> 
> Or clone with submodules:
> ```bash
> git clone --recurse-submodules https://github.com/myonathanlinkedin/paper_research.git
> ```

Comprehensive documentation is available in the [`docs/`](docs/) folder:

- [Getting Started](docs/getting-started/overview.md)
- [Architecture](docs/architecture/)
- [Features](docs/features/overview.md)
- [Research](docs/research/contributions.md)
- [Evaluation](docs/evaluation/results.md)
- [Installation](docs/installation/)

## License

This project is licensed under the Apache License 2.0. See [LICENSE](LICENSE) and [docs/license.md](docs/license.md) for details.

## Acknowledgments

- Qwen team for the LLM model
- LM Studio for the model serving infrastructure
- .NET team for the runtime environment

See [docs/acknowledgments.md](docs/acknowledgments.md) for more information.
