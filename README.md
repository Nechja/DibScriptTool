# Dib Runner

A simple command-line tool to execute C# code cells from `.dib` files using `dotnet-script`.

## Description

Dib Runner extracts C# code cells from a `.dib` (Dotnet Interactive Notebook) file and executes them sequentially using `dotnet-script`. This allows you to run your interactive notebook code directly from the command line.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [dotnet-script](https://github.com/dotnet-script/dotnet-script)

  Install `dotnet-script` globally:

  ```bash
  dotnet tool install -g DibRunner.Tool

## Usage
  ```bash
  dib-runner <path-to-your-file.dib>

## Example
  ```bash
  dib-runner MyNotebook.dib
