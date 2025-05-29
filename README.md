# RuntimeErrorSage: Intelligent Runtime Error Analysis and Remediation

RuntimeErrorSage is a .NET middleware system that enhances application reliability through intelligent runtime error analysis and remediation using local Large Language Models (LLMs). This implementation is based on the research paper described in `paper/main.tex`.

## Research Overview

RuntimeErrorSage introduces a novel approach to runtime error handling by combining:
- Graph-based context analysis
- Local LLM-powered error understanding
- Automated remediation strategies
- Model Context Protocol (MCP) for distributed context management

## Key Features

1. **Intelligent Error Analysis**
   - Context-aware error tracking
   - Rich contextual information generation
   - Natural language explanations of errors
   - Root cause identification

2. **Automated Remediation**
   - Real-time error interception
   - Intelligent remediation suggestions
   - Automated recovery strategies
   - Validation of remediation actions

3. **Privacy-Preserving Architecture**
   - Local LLM inference
   - No external service dependencies
   - Data privacy through offline operation
   - Standard HTTP API interface

4. **Middleware Integration**
   - ASP.NET Core middleware
   - Exception interception
   - Standardized error response models
   - Context management

## Key Research Contributions

### Model Context Protocol (MCP)
- Distributed context management system
- Real-time error pattern analysis
- Cross-service communication framework
- Context graph construction and analysis

### Graph-based Context Analysis
- Error pattern recognition algorithms
- Context relationship modeling
- Similarity analysis techniques
- Pattern clustering methodologies

### LLM Integration
- Integration with Qwen 2.5 7B Instruct 1M model via LM Studio
- Local error analysis and understanding
- Remediation plan generation
- Validation and verification mechanisms

### Remediation Action System
- Step-by-step remediation execution
- Action validation and safety checks
- Metrics collection and analysis
- Result tracking and reporting

## System Architecture

The system follows a service-oriented architecture with these key components:

- **RuntimeErrorSageService**: Main entry point for error handling
- **ErrorAnalyzer**: Analyzes errors using patterns and LLMs
- **RemediationService**: Handles remediation strategies
- **GraphAnalyzer**: Analyzes component dependencies
- **ContextManager**: Manages error context and state

## Implementation Details

### Prerequisites
- .NET 9 SDK and runtime
- LM Studio with Qwen 2.5 7B Instruct 1M model (local, HTTP API)
- Windows 11 (evaluation performed on Windows 11)
- Intel Core i9-13900HX CPU (or equivalent modern CPU)
- 64GB RAM (minimum for evaluation; lower may work for smaller workloads)
- NVIDIA GPU (RTX 4090 Mobile or similar, recommended for LLM inference)
- SSD storage

> **Note:** The above configuration matches the evaluation environment described in the paper. For smaller workloads, a modern CPU, 16GB+ RAM, and SSD may suffice, but performance will be lower.

### Project Structure

```
RuntimeErrorSage/
├── src/
│   ├── RuntimeErrorSage.Core/           # Core implementation
│   │   ├── Analysis/                    # Error analysis
│   │   ├── MCP/                         # Model Context Protocol
│   │   ├── LLM/                         # LLM integration
│   │   └── Remediation/                 # Remediation system
│   ├── RuntimeErrorSage.Tests/          # Test suite
│   └── RuntimeErrorSage.Examples/       # Example implementations
├── paper/                               # Research paper
│   ├── sections/                        # Paper sections
│   └── figures/                         # Paper figures
└── docs/                                # Documentation
```

## Evaluation Results

Our implementation demonstrates:
1. Error Classification
   - 80% accuracy in error classification
   - Context-aware pattern matching
   - Severity determination

2. Remediation Performance
   - 70% success rate in automated remediation
   - Average resolution time of 3.5 seconds
   - Runtime overhead under 8%

3. Test Coverage
   - 100 standardized error scenarios
   - 20 real-world error cases
   - Performance benchmark suite
   - Memory usage analysis

## Building and Running

### Setup
1. Clone the repository:
```bash
git clone https://github.com/yourusername/RuntimeErrorSage.git
cd RuntimeErrorSage
```

2. Install dependencies:
```bash
dotnet restore
```

3. Configure LM Studio:
- Download and install LM Studio
- Load the Qwen 2.5 7B Instruct 1M model
- Update `appsettings.json` with your LM Studio configuration

4. Build the solution:
```bash
dotnet build
```

### Running Tests
```bash
dotnet test
```

## Research Paper

The implementation is based on our research paper "RuntimeErrorSage: Intelligent Runtime Error Analysis and Remediation using Local Large Language Models". The paper describes:

1. System Architecture
   - Model Context Protocol
   - Graph-based analysis
   - Local LLM integration
   - Remediation system

2. Implementation Details
   - Error detection and analysis
   - Context management
   - Pattern recognition
   - Action execution

3. Evaluation Results
   - Test suite performance
   - Accuracy metrics
   - Performance benchmarks
   - Resource utilization

## Contributing

This is a research project. Contributions should focus on:
- Improving the theoretical foundation
- Enhancing the implementation
- Adding new test cases
- Documenting findings

## 📜 License - Apache License 2.0

This project follows the Apache License 2.0, which means:

- ✅ You can use, modify, and distribute the code freely.
- ✅ You must include the original license when distributing.
- ✅ You must include the NOTICE file if one is provided.
- ✅ You can use this in personal & commercial projects.
- ✅ No warranties – use at your own risk!

For full details, see the [Apache License 2.0](https://www.apache.org/licenses/LICENSE-2.0).

## Acknowledgments

- Qwen team for the LLM model
- LM Studio for the model serving infrastructure
- .NET team for the runtime environment
