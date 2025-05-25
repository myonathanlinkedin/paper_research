# CodeSage: A Local LLM-Assisted Runtime Intelligence Layer

## Overview
CodeSage is a novel runtime middleware layer for .NET 9 applications that enhances software reliability by leveraging local Large Language Models (LLMs) through LM Studio API and Model Context Protocol (MCP) for intelligent error handling and self-healing capabilities.

## Key Features
- Real-time exception interception and analysis
- Natural language error explanations
- Actionable remediation suggestions
- Self-healing mechanisms
- Privacy-preserving local LLM inference
- MCP-enabled distributed context sharing

## Project Structure
```
paper_research/
├── paper/                    # IEEE format research paper
│   ├── main.tex             # Main LaTeX document
│   ├── sections/            # Paper sections
│   └── figures/             # Diagrams and figures
├── src/                     # Implementation
│   ├── CodeSage.Core/       # Core middleware components
│   ├── CodeSage.MCP/        # Model Context Protocol implementation
│   └── CodeSage.LMStudio/   # LM Studio API integration
└── tests/                   # Test projects
```

## Research Components

### 1. Runtime Intelligence Layer
- Exception interception middleware
- Context-aware error analysis
- Natural language processing of error states
- Automated remediation strategies

### 2. Model Context Protocol (MCP)
- Standardized context sharing
- Distributed metadata management
- Inter-service communication protocols
- Context enrichment mechanisms

### 3. LM Studio Integration
- Local LLM inference
- Privacy-preserving processing
- Edge computing support
- Custom model fine-tuning

## Implementation Details
- Built on .NET 9
- Integrates with LM Studio API
- Implements MCP for distributed systems
- Supports ASP.NET Core middleware
- Provides extensible plugin architecture

## Getting Started
1. Clone the repository
2. Install .NET 9 SDK
3. Set up LM Studio with a compatible model (e.g., Mistral)
4. Configure MCP endpoints
5. Build and run the solution

## Author
Mateus Yonathan
- LinkedIn: [Mateus Yonathan](https://www.linkedin.com/in/siyoyo/)
- Independent Researcher

## License
This research is licensed under the MIT License - see the LICENSE file for details.

## Citation
If you use this research in your work, please cite:
```
@article{codesage2024,
  title={CodeSage: A Local Large Language Model Assisted Runtime Intelligence Layer with Model Context Protocol for Self Healing Application Errors in .NET Environments Using LM Studio API},
  author={Yonathan, Mateus},
  journal={arXiv preprint},
  year={2024}
}
``` 