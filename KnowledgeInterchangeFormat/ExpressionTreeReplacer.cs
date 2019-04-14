// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat
{
    using System.Collections.Immutable;
    using KnowledgeInterchangeFormat.Expressions;

    /*
     * Generated with the following script:
     *
        var assembly = Assembly.LoadFrom(path);
        var expressionType = assembly.GetType("KnowledgeInterchangeFormat.Expressions.Expression");
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(expressionType);
        string typeName(Type t) => t.Name;
        var types = assembly.GetTypes().OrderBy(typeName).Where(t => expressionType.IsAssignableFrom(t)).ToList();
        string e(string name) => (name == "operator" || name == "default") ? "@" + name : name;
        string lowerFirst(string name) => e(char.ToLowerInvariant(name[0]) + name.Substring(1));

        var descendents = types.ToLookup(t => t.BaseType);
        foreach (var type in types)
        {
            var name = type.Name;
            $"/// <summary>".Dump();
            $"/// Walks the <see cref=\"{name}\"/>.".Dump();
            $"/// </summary>".Dump();
            $"/// <param name=\"{lowerFirst(name).TrimStart('@')}\">The <see cref=\"{name}\"/> to walk.</param>".Dump();
            $"/// <param name=\"path\">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>".Dump();
            $"/// <returns>A replacement expression.</returns>".Dump();
            $"public virtual Expression Walk({name} {lowerFirst(name)}, ImmutableStack<string> path{(type == expressionType ? " = null" : "")})".Dump();
            "{".Dump();

            if (type == expressionType)
            {
                "    if (path == null)".Dump();
                "    {".Dump();
                "        path = ImmutableStack<string>.Empty.Push(\"$\");".Dump();
                "    }".Dump();
                "".Dump();
            }

            if (descendents[type].Any())
            {
                $"    switch ({lowerFirst(name)})".Dump();
                "    {".Dump();
                foreach (var descendent in descendents[type].OrderBy(typeName))
                {
                    $"        case {descendent.Name} {lowerFirst(descendent.Name)}:".Dump();
                    $"            {lowerFirst(name)} = {(type == expressionType ? "" : $"({name})")}this.Walk({lowerFirst(descendent.Name)}, path);".Dump();
                    if (!type.IsAbstract)
                    {
                        $"            return {lowerFirst(name)};".Dump();
                    }
                    else
                    {
                        "            break;".Dump();
                    }
                }
                "    }".Dump();
                "".Dump();
            }

            if (type.Name == "CharacterString" || type.Name == "CharacterBlock")
            {
                // These types are special cases of Lists that can be considered atomic.
            }
            else if (!type.IsAbstract)
            {
                var constructor = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).First();
                var constructorParams = constructor.GetParameters();
                var constructorParamNames = constructorParams.Select(p => p.Name.ToUpperInvariant()).ToList();
                var properties = (from p in type.GetProperties()
                                  where p.DeclaringType == type || !type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).Any(d => d.Name == p.Name)
                                  orderby (uint)constructorParamNames.IndexOf(p.Name.ToUpperInvariant())
                                  select new
                                  {
                                      p.Name,
                                      Type = p.PropertyType,
                                      Var = lowerFirst(p.Name),
                                  }).ToList();
                var walkableProperties = properties.Where(p => expressionType.IsAssignableFrom(p.Type) || enumerableType.IsAssignableFrom(p.Type));
                if (walkableProperties.Any())
                {
                    var first = true;
                    foreach (var property in walkableProperties)
                    {
                        if (!first) "".Dump();
                        first = false;
                        $"    var {property.Var} = {lowerFirst(name)}.{property.Name};".Dump();
                        $"    if ({property.Var} is object)".Dump();
                        "    {".Dump();
                        if (expressionType.IsAssignableFrom(property.Type))
                        {
                            $"        {property.Var} = {(property.Type == expressionType ? "" : $"({property.Type.Name})")}this.Walk({(property.Type == expressionType ? "" : "(Expression)")}{lowerFirst(name)}.{property.Name}, path.Push(\".{property.Name}\"));".Dump();
                        }
                        else if (enumerableType.IsAssignableFrom(property.Type))
                        {
                            var subType = property.Type.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)).Single().GenericTypeArguments.Single();
                            var subTypeName = subType == type ? "sub" + type.Name : lowerFirst(subType.Name);
                            $"        for (var i = {property.Var}.Count - 1; i >= 0; i--)".Dump();
                            "        {".Dump();
                            $"            var {subTypeName} = {(subType == expressionType ? "" : $"({subType.Name})")}this.Walk({(subType == expressionType ? "" : "(Expression)")}{property.Var}[i], path.Push($\".{property.Name}[{{i}}]\"));".Dump();
                            $"            if ({subTypeName} is null)".Dump();
                            "            {".Dump();
                            $"                {property.Var} = {property.Var}.RemoveAt(i);".Dump();
                            "            }".Dump();
                            $"            else if ({subTypeName} != {property.Var}[i])".Dump();
                            "            {".Dump();
                            $"                {property.Var} = {property.Var}.SetItem(i, {subTypeName});".Dump();
                            "            }".Dump();
                            "        }".Dump();
                        }
                        "    }".Dump();
                    }

                    "".Dump();
                    $"    if ({string.Join($" ||{Environment.NewLine}        ", walkableProperties.Select(property => $"{property.Var} != {lowerFirst(name)}.{property.Name}"))})".Dump();
                    "    {".Dump();
                    $"        return new {name}(".Dump();
                    var args = from a in constructorParams
                               let prop = properties.Single(p => p.Name.ToUpperInvariant() == a.Name.ToUpperInvariant())
                               let walked = walkableProperties.SingleOrDefault(p => p.Name.ToUpperInvariant() == a.Name.ToUpperInvariant())
                               select new
                               {
                                   a.Name,
                                   Value = walked?.Var ?? $"{lowerFirst(name)}.{prop.Name}"
                               };
                    $"            {string.Join($",{Environment.NewLine}            ", args.Select(p => $"{e(p.Name)}: {p.Value}"))});".Dump();
                    "    }".Dump();
                    "".Dump();
                }
            }
            $"    return {lowerFirst(name)};".Dump();
            "}".Dump();
            "".Dump();
        }
    */

    /// <summary>
    /// A base class for walking expression trees while replacing elements.
    /// </summary>
    public class ExpressionTreeReplacer
    {
        /// <summary>
        /// Walks the <see cref="CharacterBlock"/>.
        /// </summary>
        /// <param name="characterBlock">The <see cref="CharacterBlock"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(CharacterBlock characterBlock, ImmutableStack<string> path)
        {
            return characterBlock;
        }

        /// <summary>
        /// Walks the <see cref="CharacterReference"/>.
        /// </summary>
        /// <param name="characterReference">The <see cref="CharacterReference"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(CharacterReference characterReference, ImmutableStack<string> path)
        {
            return characterReference;
        }

        /// <summary>
        /// Walks the <see cref="CharacterString"/>.
        /// </summary>
        /// <param name="characterString">The <see cref="CharacterString"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(CharacterString characterString, ImmutableStack<string> path)
        {
            return characterString;
        }

        /// <summary>
        /// Walks the <see cref="CompleteDefinition"/>.
        /// </summary>
        /// <param name="completeDefinition">The <see cref="CompleteDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(CompleteDefinition completeDefinition, ImmutableStack<string> path)
        {
            switch (completeDefinition)
            {
                case CompleteFunctionDefinition completeFunctionDefinition:
                    completeDefinition = (CompleteDefinition)this.Walk(completeFunctionDefinition, path);
                    break;
                case CompleteLogicalDefinition completeLogicalDefinition:
                    completeDefinition = (CompleteDefinition)this.Walk(completeLogicalDefinition, path);
                    break;
                case CompleteObjectDefinition completeObjectDefinition:
                    completeDefinition = (CompleteDefinition)this.Walk(completeObjectDefinition, path);
                    break;
                case CompleteRelationDefinition completeRelationDefinition:
                    completeDefinition = (CompleteDefinition)this.Walk(completeRelationDefinition, path);
                    break;
            }

            return completeDefinition;
        }

        /// <summary>
        /// Walks the <see cref="CompleteFunctionDefinition"/>.
        /// </summary>
        /// <param name="completeFunctionDefinition">The <see cref="CompleteFunctionDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(CompleteFunctionDefinition completeFunctionDefinition, ImmutableStack<string> path)
        {
            var constant = completeFunctionDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)completeFunctionDefinition.Constant, path.Push(".Constant"));
            }

            var description = completeFunctionDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)completeFunctionDefinition.Description, path.Push(".Description"));
            }

            var parameters = completeFunctionDefinition.Parameters;
            if (parameters is object)
            {
                for (var i = parameters.Count - 1; i >= 0; i--)
                {
                    var individualVariable = (IndividualVariable)this.Walk((Expression)parameters[i], path.Push($".Parameters[{i}]"));
                    if (individualVariable is null)
                    {
                        parameters = parameters.RemoveAt(i);
                    }
                    else if (individualVariable != parameters[i])
                    {
                        parameters = parameters.SetItem(i, individualVariable);
                    }
                }
            }

            var sequenceVariable = completeFunctionDefinition.SequenceVariable;
            if (sequenceVariable is object)
            {
                sequenceVariable = (SequenceVariable)this.Walk((Expression)completeFunctionDefinition.SequenceVariable, path.Push(".SequenceVariable"));
            }

            var term = completeFunctionDefinition.Term;
            if (term is object)
            {
                term = (Term)this.Walk((Expression)completeFunctionDefinition.Term, path.Push(".Term"));
            }

            if (constant != completeFunctionDefinition.Constant ||
                description != completeFunctionDefinition.Description ||
                parameters != completeFunctionDefinition.Parameters ||
                sequenceVariable != completeFunctionDefinition.SequenceVariable ||
                term != completeFunctionDefinition.Term)
            {
                return new CompleteFunctionDefinition(
                    constant: constant,
                    description: description,
                    parameters: parameters,
                    sequenceVariable: sequenceVariable,
                    term: term);
            }

            return completeFunctionDefinition;
        }

        /// <summary>
        /// Walks the <see cref="CompleteLogicalDefinition"/>.
        /// </summary>
        /// <param name="completeLogicalDefinition">The <see cref="CompleteLogicalDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(CompleteLogicalDefinition completeLogicalDefinition, ImmutableStack<string> path)
        {
            var constant = completeLogicalDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)completeLogicalDefinition.Constant, path.Push(".Constant"));
            }

            var description = completeLogicalDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)completeLogicalDefinition.Description, path.Push(".Description"));
            }

            var sentence = completeLogicalDefinition.Sentence;
            if (sentence is object)
            {
                sentence = (Sentence)this.Walk((Expression)completeLogicalDefinition.Sentence, path.Push(".Sentence"));
            }

            if (constant != completeLogicalDefinition.Constant ||
                description != completeLogicalDefinition.Description ||
                sentence != completeLogicalDefinition.Sentence)
            {
                return new CompleteLogicalDefinition(
                    constant: constant,
                    description: description,
                    sentence: sentence);
            }

            return completeLogicalDefinition;
        }

        /// <summary>
        /// Walks the <see cref="CompleteObjectDefinition"/>.
        /// </summary>
        /// <param name="completeObjectDefinition">The <see cref="CompleteObjectDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(CompleteObjectDefinition completeObjectDefinition, ImmutableStack<string> path)
        {
            var constant = completeObjectDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)completeObjectDefinition.Constant, path.Push(".Constant"));
            }

            var description = completeObjectDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)completeObjectDefinition.Description, path.Push(".Description"));
            }

            var term = completeObjectDefinition.Term;
            if (term is object)
            {
                term = (Term)this.Walk((Expression)completeObjectDefinition.Term, path.Push(".Term"));
            }

            if (constant != completeObjectDefinition.Constant ||
                description != completeObjectDefinition.Description ||
                term != completeObjectDefinition.Term)
            {
                return new CompleteObjectDefinition(
                    constant: constant,
                    description: description,
                    term: term);
            }

            return completeObjectDefinition;
        }

        /// <summary>
        /// Walks the <see cref="CompleteRelationDefinition"/>.
        /// </summary>
        /// <param name="completeRelationDefinition">The <see cref="CompleteRelationDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(CompleteRelationDefinition completeRelationDefinition, ImmutableStack<string> path)
        {
            var constant = completeRelationDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)completeRelationDefinition.Constant, path.Push(".Constant"));
            }

            var description = completeRelationDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)completeRelationDefinition.Description, path.Push(".Description"));
            }

            var parameters = completeRelationDefinition.Parameters;
            if (parameters is object)
            {
                for (var i = parameters.Count - 1; i >= 0; i--)
                {
                    var individualVariable = (IndividualVariable)this.Walk((Expression)parameters[i], path.Push($".Parameters[{i}]"));
                    if (individualVariable is null)
                    {
                        parameters = parameters.RemoveAt(i);
                    }
                    else if (individualVariable != parameters[i])
                    {
                        parameters = parameters.SetItem(i, individualVariable);
                    }
                }
            }

            var sequenceVariable = completeRelationDefinition.SequenceVariable;
            if (sequenceVariable is object)
            {
                sequenceVariable = (SequenceVariable)this.Walk((Expression)completeRelationDefinition.SequenceVariable, path.Push(".SequenceVariable"));
            }

            var sentence = completeRelationDefinition.Sentence;
            if (sentence is object)
            {
                sentence = (Sentence)this.Walk((Expression)completeRelationDefinition.Sentence, path.Push(".Sentence"));
            }

            if (constant != completeRelationDefinition.Constant ||
                description != completeRelationDefinition.Description ||
                parameters != completeRelationDefinition.Parameters ||
                sequenceVariable != completeRelationDefinition.SequenceVariable ||
                sentence != completeRelationDefinition.Sentence)
            {
                return new CompleteRelationDefinition(
                    constant: constant,
                    description: description,
                    parameters: parameters,
                    sequenceVariable: sequenceVariable,
                    sentence: sentence);
            }

            return completeRelationDefinition;
        }

        /// <summary>
        /// Walks the <see cref="ConditionalTerm"/>.
        /// </summary>
        /// <param name="conditionalTerm">The <see cref="ConditionalTerm"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ConditionalTerm conditionalTerm, ImmutableStack<string> path)
        {
            var pairs = conditionalTerm.Pairs;
            if (pairs is object)
            {
                for (var i = pairs.Count - 1; i >= 0; i--)
                {
                    var logicalPair = (LogicalPair)this.Walk((Expression)pairs[i], path.Push($".Pairs[{i}]"));
                    if (logicalPair is null)
                    {
                        pairs = pairs.RemoveAt(i);
                    }
                    else if (logicalPair != pairs[i])
                    {
                        pairs = pairs.SetItem(i, logicalPair);
                    }
                }
            }

            if (pairs != conditionalTerm.Pairs)
            {
                return new ConditionalTerm(
                    pairs: pairs);
            }

            return conditionalTerm;
        }

        /// <summary>
        /// Walks the <see cref="Conjunction"/>.
        /// </summary>
        /// <param name="conjunction">The <see cref="Conjunction"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Conjunction conjunction, ImmutableStack<string> path)
        {
            var conjuncts = conjunction.Conjuncts;
            if (conjuncts is object)
            {
                for (var i = conjuncts.Count - 1; i >= 0; i--)
                {
                    var sentence = (Sentence)this.Walk((Expression)conjuncts[i], path.Push($".Conjuncts[{i}]"));
                    if (sentence is null)
                    {
                        conjuncts = conjuncts.RemoveAt(i);
                    }
                    else if (sentence != conjuncts[i])
                    {
                        conjuncts = conjuncts.SetItem(i, sentence);
                    }
                }
            }

            if (conjuncts != conjunction.Conjuncts)
            {
                return new Conjunction(
                    conjuncts: conjuncts);
            }

            return conjunction;
        }

        /// <summary>
        /// Walks the <see cref="Constant"/>.
        /// </summary>
        /// <param name="constant">The <see cref="Constant"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Constant constant, ImmutableStack<string> path)
        {
            return constant;
        }

        /// <summary>
        /// Walks the <see cref="ConstantSentence"/>.
        /// </summary>
        /// <param name="constantSentence">The <see cref="ConstantSentence"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ConstantSentence constantSentence, ImmutableStack<string> path)
        {
            var constant = constantSentence.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)constantSentence.Constant, path.Push(".Constant"));
            }

            if (constant != constantSentence.Constant)
            {
                return new ConstantSentence(
                    constant: constant);
            }

            return constantSentence;
        }

        /// <summary>
        /// Walks the <see cref="Definition"/>.
        /// </summary>
        /// <param name="definition">The <see cref="Definition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Definition definition, ImmutableStack<string> path)
        {
            switch (definition)
            {
                case CompleteDefinition completeDefinition:
                    definition = (Definition)this.Walk(completeDefinition, path);
                    break;
                case PartialDefinition partialDefinition:
                    definition = (Definition)this.Walk(partialDefinition, path);
                    break;
                case UnrestrictedDefinition unrestrictedDefinition:
                    definition = (Definition)this.Walk(unrestrictedDefinition, path);
                    break;
            }

            return definition;
        }

        /// <summary>
        /// Walks the <see cref="Disjunction"/>.
        /// </summary>
        /// <param name="disjunction">The <see cref="Disjunction"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Disjunction disjunction, ImmutableStack<string> path)
        {
            var disjuncts = disjunction.Disjuncts;
            if (disjuncts is object)
            {
                for (var i = disjuncts.Count - 1; i >= 0; i--)
                {
                    var sentence = (Sentence)this.Walk((Expression)disjuncts[i], path.Push($".Disjuncts[{i}]"));
                    if (sentence is null)
                    {
                        disjuncts = disjuncts.RemoveAt(i);
                    }
                    else if (sentence != disjuncts[i])
                    {
                        disjuncts = disjuncts.SetItem(i, sentence);
                    }
                }
            }

            if (disjuncts != disjunction.Disjuncts)
            {
                return new Disjunction(
                    disjuncts: disjuncts);
            }

            return disjunction;
        }

        /// <summary>
        /// Walks the <see cref="Equation"/>.
        /// </summary>
        /// <param name="equation">The <see cref="Equation"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Equation equation, ImmutableStack<string> path)
        {
            var left = equation.Left;
            if (left is object)
            {
                left = (Term)this.Walk((Expression)equation.Left, path.Push(".Left"));
            }

            var right = equation.Right;
            if (right is object)
            {
                right = (Term)this.Walk((Expression)equation.Right, path.Push(".Right"));
            }

            if (left != equation.Left ||
                right != equation.Right)
            {
                return new Equation(
                    left: left,
                    right: right);
            }

            return equation;
        }

        /// <summary>
        /// Walks the <see cref="Equivalence"/>.
        /// </summary>
        /// <param name="equivalence">The <see cref="Equivalence"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Equivalence equivalence, ImmutableStack<string> path)
        {
            var left = equivalence.Left;
            if (left is object)
            {
                left = (Sentence)this.Walk((Expression)equivalence.Left, path.Push(".Left"));
            }

            var right = equivalence.Right;
            if (right is object)
            {
                right = (Sentence)this.Walk((Expression)equivalence.Right, path.Push(".Right"));
            }

            if (left != equivalence.Left ||
                right != equivalence.Right)
            {
                return new Equivalence(
                    left: left,
                    right: right);
            }

            return equivalence;
        }

        /// <summary>
        /// Walks the <see cref="ExistentiallyQuantifiedSentence"/>.
        /// </summary>
        /// <param name="existentiallyQuantifiedSentence">The <see cref="ExistentiallyQuantifiedSentence"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ExistentiallyQuantifiedSentence existentiallyQuantifiedSentence, ImmutableStack<string> path)
        {
            var variables = existentiallyQuantifiedSentence.Variables;
            if (variables is object)
            {
                for (var i = variables.Count - 1; i >= 0; i--)
                {
                    var variableSpecification = (VariableSpecification)this.Walk((Expression)variables[i], path.Push($".Variables[{i}]"));
                    if (variableSpecification is null)
                    {
                        variables = variables.RemoveAt(i);
                    }
                    else if (variableSpecification != variables[i])
                    {
                        variables = variables.SetItem(i, variableSpecification);
                    }
                }
            }

            var quantified = existentiallyQuantifiedSentence.Quantified;
            if (quantified is object)
            {
                quantified = (Sentence)this.Walk((Expression)existentiallyQuantifiedSentence.Quantified, path.Push(".Quantified"));
            }

            if (variables != existentiallyQuantifiedSentence.Variables ||
                quantified != existentiallyQuantifiedSentence.Quantified)
            {
                return new ExistentiallyQuantifiedSentence(
                    variables: variables,
                    quantified: quantified);
            }

            return existentiallyQuantifiedSentence;
        }

        /// <summary>
        /// Walks the <see cref="ExplicitFunctionalTerm"/>.
        /// </summary>
        /// <param name="explicitFunctionalTerm">The <see cref="ExplicitFunctionalTerm"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ExplicitFunctionalTerm explicitFunctionalTerm, ImmutableStack<string> path)
        {
            var function = explicitFunctionalTerm.Function;
            if (function is object)
            {
                function = (Term)this.Walk((Expression)explicitFunctionalTerm.Function, path.Push(".Function"));
            }

            var arguments = explicitFunctionalTerm.Arguments;
            if (arguments is object)
            {
                for (var i = arguments.Count - 1; i >= 0; i--)
                {
                    var term = (Term)this.Walk((Expression)arguments[i], path.Push($".Arguments[{i}]"));
                    if (term is null)
                    {
                        arguments = arguments.RemoveAt(i);
                    }
                    else if (term != arguments[i])
                    {
                        arguments = arguments.SetItem(i, term);
                    }
                }
            }

            var sequenceVariable = explicitFunctionalTerm.SequenceVariable;
            if (sequenceVariable is object)
            {
                sequenceVariable = (SequenceVariable)this.Walk((Expression)explicitFunctionalTerm.SequenceVariable, path.Push(".SequenceVariable"));
            }

            if (function != explicitFunctionalTerm.Function ||
                arguments != explicitFunctionalTerm.Arguments ||
                sequenceVariable != explicitFunctionalTerm.SequenceVariable)
            {
                return new ExplicitFunctionalTerm(
                    function: function,
                    arguments: arguments,
                    sequenceVariable: sequenceVariable);
            }

            return explicitFunctionalTerm;
        }

        /// <summary>
        /// Walks the <see cref="ExplicitRelationalSentence"/>.
        /// </summary>
        /// <param name="explicitRelationalSentence">The <see cref="ExplicitRelationalSentence"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ExplicitRelationalSentence explicitRelationalSentence, ImmutableStack<string> path)
        {
            var relation = explicitRelationalSentence.Relation;
            if (relation is object)
            {
                relation = (Term)this.Walk((Expression)explicitRelationalSentence.Relation, path.Push(".Relation"));
            }

            var arguments = explicitRelationalSentence.Arguments;
            if (arguments is object)
            {
                for (var i = arguments.Count - 1; i >= 0; i--)
                {
                    var term = (Term)this.Walk((Expression)arguments[i], path.Push($".Arguments[{i}]"));
                    if (term is null)
                    {
                        arguments = arguments.RemoveAt(i);
                    }
                    else if (term != arguments[i])
                    {
                        arguments = arguments.SetItem(i, term);
                    }
                }
            }

            var sequenceVariable = explicitRelationalSentence.SequenceVariable;
            if (sequenceVariable is object)
            {
                sequenceVariable = (SequenceVariable)this.Walk((Expression)explicitRelationalSentence.SequenceVariable, path.Push(".SequenceVariable"));
            }

            if (relation != explicitRelationalSentence.Relation ||
                arguments != explicitRelationalSentence.Arguments ||
                sequenceVariable != explicitRelationalSentence.SequenceVariable)
            {
                return new ExplicitRelationalSentence(
                    relation: relation,
                    arguments: arguments,
                    sequenceVariable: sequenceVariable);
            }

            return explicitRelationalSentence;
        }

        /// <summary>
        /// Walks the <see cref="Expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Expression expression, ImmutableStack<string> path = null)
        {
            if (path == null)
            {
                path = ImmutableStack<string>.Empty.Push("$");
            }

            switch (expression)
            {
                case Form form:
                    expression = this.Walk(form, path);
                    break;
                case KnowledgeBase knowledgeBase:
                    expression = this.Walk(knowledgeBase, path);
                    break;
                case ListExpression listExpression:
                    expression = this.Walk(listExpression, path);
                    break;
                case LogicalPair logicalPair:
                    expression = this.Walk(logicalPair, path);
                    break;
                case Term term:
                    expression = this.Walk(term, path);
                    break;
                case VariableSpecification variableSpecification:
                    expression = this.Walk(variableSpecification, path);
                    break;
            }

            return expression;
        }

        /// <summary>
        /// Walks the <see cref="Form"/>.
        /// </summary>
        /// <param name="form">The <see cref="Form"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Form form, ImmutableStack<string> path)
        {
            switch (form)
            {
                case Definition definition:
                    form = (Form)this.Walk(definition, path);
                    break;
                case Sentence sentence:
                    form = (Form)this.Walk(sentence, path);
                    break;
            }

            return form;
        }

        /// <summary>
        /// Walks the <see cref="FunctionalTerm"/>.
        /// </summary>
        /// <param name="functionalTerm">The <see cref="FunctionalTerm"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(FunctionalTerm functionalTerm, ImmutableStack<string> path)
        {
            switch (functionalTerm)
            {
                case ExplicitFunctionalTerm explicitFunctionalTerm:
                    functionalTerm = (FunctionalTerm)this.Walk(explicitFunctionalTerm, path);
                    break;
                case ImplicitFunctionalTerm implicitFunctionalTerm:
                    functionalTerm = (FunctionalTerm)this.Walk(implicitFunctionalTerm, path);
                    break;
            }

            return functionalTerm;
        }

        /// <summary>
        /// Walks the <see cref="IfTerm"/>.
        /// </summary>
        /// <param name="ifTerm">The <see cref="IfTerm"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(IfTerm ifTerm, ImmutableStack<string> path)
        {
            var pairs = ifTerm.Pairs;
            if (pairs is object)
            {
                for (var i = pairs.Count - 1; i >= 0; i--)
                {
                    var logicalPair = (LogicalPair)this.Walk((Expression)pairs[i], path.Push($".Pairs[{i}]"));
                    if (logicalPair is null)
                    {
                        pairs = pairs.RemoveAt(i);
                    }
                    else if (logicalPair != pairs[i])
                    {
                        pairs = pairs.SetItem(i, logicalPair);
                    }
                }
            }

            var @default = ifTerm.Default;
            if (@default is object)
            {
                @default = (Term)this.Walk((Expression)ifTerm.Default, path.Push(".Default"));
            }

            if (pairs != ifTerm.Pairs ||
                @default != ifTerm.Default)
            {
                return new IfTerm(
                    pairs: pairs,
                    @default: @default);
            }

            return ifTerm;
        }

        /// <summary>
        /// Walks the <see cref="Implication"/>.
        /// </summary>
        /// <param name="implication">The <see cref="Implication"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Implication implication, ImmutableStack<string> path)
        {
            switch (implication)
            {
                case ReverseImplication reverseImplication:
                    implication = (Implication)this.Walk(reverseImplication, path);
                    return implication;
            }

            var antecedents = implication.Antecedents;
            if (antecedents is object)
            {
                for (var i = antecedents.Count - 1; i >= 0; i--)
                {
                    var sentence = (Sentence)this.Walk((Expression)antecedents[i], path.Push($".Antecedents[{i}]"));
                    if (sentence is null)
                    {
                        antecedents = antecedents.RemoveAt(i);
                    }
                    else if (sentence != antecedents[i])
                    {
                        antecedents = antecedents.SetItem(i, sentence);
                    }
                }
            }

            var consequent = implication.Consequent;
            if (consequent is object)
            {
                consequent = (Sentence)this.Walk((Expression)implication.Consequent, path.Push(".Consequent"));
            }

            if (antecedents != implication.Antecedents ||
                consequent != implication.Consequent)
            {
                return new Implication(
                    antecedents: antecedents,
                    consequent: consequent);
            }

            return implication;
        }

        /// <summary>
        /// Walks the <see cref="ImplicitFunctionalTerm"/>.
        /// </summary>
        /// <param name="implicitFunctionalTerm">The <see cref="ImplicitFunctionalTerm"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ImplicitFunctionalTerm implicitFunctionalTerm, ImmutableStack<string> path)
        {
            var function = implicitFunctionalTerm.Function;
            if (function is object)
            {
                function = (Constant)this.Walk((Expression)implicitFunctionalTerm.Function, path.Push(".Function"));
            }

            var arguments = implicitFunctionalTerm.Arguments;
            if (arguments is object)
            {
                for (var i = arguments.Count - 1; i >= 0; i--)
                {
                    var term = (Term)this.Walk((Expression)arguments[i], path.Push($".Arguments[{i}]"));
                    if (term is null)
                    {
                        arguments = arguments.RemoveAt(i);
                    }
                    else if (term != arguments[i])
                    {
                        arguments = arguments.SetItem(i, term);
                    }
                }
            }

            var sequenceVariable = implicitFunctionalTerm.SequenceVariable;
            if (sequenceVariable is object)
            {
                sequenceVariable = (SequenceVariable)this.Walk((Expression)implicitFunctionalTerm.SequenceVariable, path.Push(".SequenceVariable"));
            }

            if (function != implicitFunctionalTerm.Function ||
                arguments != implicitFunctionalTerm.Arguments ||
                sequenceVariable != implicitFunctionalTerm.SequenceVariable)
            {
                return new ImplicitFunctionalTerm(
                    function: function,
                    arguments: arguments,
                    sequenceVariable: sequenceVariable);
            }

            return implicitFunctionalTerm;
        }

        /// <summary>
        /// Walks the <see cref="ImplicitRelationalSentence"/>.
        /// </summary>
        /// <param name="implicitRelationalSentence">The <see cref="ImplicitRelationalSentence"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ImplicitRelationalSentence implicitRelationalSentence, ImmutableStack<string> path)
        {
            var relation = implicitRelationalSentence.Relation;
            if (relation is object)
            {
                relation = (Constant)this.Walk((Expression)implicitRelationalSentence.Relation, path.Push(".Relation"));
            }

            var arguments = implicitRelationalSentence.Arguments;
            if (arguments is object)
            {
                for (var i = arguments.Count - 1; i >= 0; i--)
                {
                    var term = (Term)this.Walk((Expression)arguments[i], path.Push($".Arguments[{i}]"));
                    if (term is null)
                    {
                        arguments = arguments.RemoveAt(i);
                    }
                    else if (term != arguments[i])
                    {
                        arguments = arguments.SetItem(i, term);
                    }
                }
            }

            var sequenceVariable = implicitRelationalSentence.SequenceVariable;
            if (sequenceVariable is object)
            {
                sequenceVariable = (SequenceVariable)this.Walk((Expression)implicitRelationalSentence.SequenceVariable, path.Push(".SequenceVariable"));
            }

            if (relation != implicitRelationalSentence.Relation ||
                arguments != implicitRelationalSentence.Arguments ||
                sequenceVariable != implicitRelationalSentence.SequenceVariable)
            {
                return new ImplicitRelationalSentence(
                    relation: relation,
                    arguments: arguments,
                    sequenceVariable: sequenceVariable);
            }

            return implicitRelationalSentence;
        }

        /// <summary>
        /// Walks the <see cref="IndividualVariable"/>.
        /// </summary>
        /// <param name="individualVariable">The <see cref="IndividualVariable"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(IndividualVariable individualVariable, ImmutableStack<string> path)
        {
            return individualVariable;
        }

        /// <summary>
        /// Walks the <see cref="Inequality"/>.
        /// </summary>
        /// <param name="inequality">The <see cref="Inequality"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Inequality inequality, ImmutableStack<string> path)
        {
            var left = inequality.Left;
            if (left is object)
            {
                left = (Term)this.Walk((Expression)inequality.Left, path.Push(".Left"));
            }

            var right = inequality.Right;
            if (right is object)
            {
                right = (Term)this.Walk((Expression)inequality.Right, path.Push(".Right"));
            }

            if (left != inequality.Left ||
                right != inequality.Right)
            {
                return new Inequality(
                    left: left,
                    right: right);
            }

            return inequality;
        }

        /// <summary>
        /// Walks the <see cref="KnowledgeBase"/>.
        /// </summary>
        /// <param name="knowledgeBase">The <see cref="KnowledgeBase"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(KnowledgeBase knowledgeBase, ImmutableStack<string> path)
        {
            var forms = knowledgeBase.Forms;
            if (forms is object)
            {
                for (var i = forms.Count - 1; i >= 0; i--)
                {
                    var form = (Form)this.Walk((Expression)forms[i], path.Push($".Forms[{i}]"));
                    if (form is null)
                    {
                        forms = forms.RemoveAt(i);
                    }
                    else if (form != forms[i])
                    {
                        forms = forms.SetItem(i, form);
                    }
                }
            }

            if (forms != knowledgeBase.Forms)
            {
                return new KnowledgeBase(
                    forms: forms);
            }

            return knowledgeBase;
        }

        /// <summary>
        /// Walks the <see cref="ListExpression"/>.
        /// </summary>
        /// <param name="listExpression">The <see cref="ListExpression"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ListExpression listExpression, ImmutableStack<string> path)
        {
            var items = listExpression.Items;
            if (items is object)
            {
                for (var i = items.Count - 1; i >= 0; i--)
                {
                    var expression = this.Walk(items[i], path.Push($".Items[{i}]"));
                    if (expression is null)
                    {
                        items = items.RemoveAt(i);
                    }
                    else if (expression != items[i])
                    {
                        items = items.SetItem(i, expression);
                    }
                }
            }

            if (items != listExpression.Items)
            {
                return new ListExpression(
                    items: items);
            }

            return listExpression;
        }

        /// <summary>
        /// Walks the <see cref="ListTerm"/>.
        /// </summary>
        /// <param name="listTerm">The <see cref="ListTerm"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ListTerm listTerm, ImmutableStack<string> path)
        {
            switch (listTerm)
            {
                case CharacterBlock characterBlock:
                    listTerm = (ListTerm)this.Walk(characterBlock, path);
                    return listTerm;
                case CharacterString characterString:
                    listTerm = (ListTerm)this.Walk(characterString, path);
                    return listTerm;
            }

            var items = listTerm.Items;
            if (items is object)
            {
                for (var i = items.Count - 1; i >= 0; i--)
                {
                    var term = (Term)this.Walk((Expression)items[i], path.Push($".Items[{i}]"));
                    if (term is null)
                    {
                        items = items.RemoveAt(i);
                    }
                    else if (term != items[i])
                    {
                        items = items.SetItem(i, term);
                    }
                }
            }

            var sequenceVariable = listTerm.SequenceVariable;
            if (sequenceVariable is object)
            {
                sequenceVariable = (SequenceVariable)this.Walk((Expression)listTerm.SequenceVariable, path.Push(".SequenceVariable"));
            }

            if (items != listTerm.Items ||
                sequenceVariable != listTerm.SequenceVariable)
            {
                return new ListTerm(
                    items: items,
                    sequenceVariable: sequenceVariable);
            }

            return listTerm;
        }

        /// <summary>
        /// Walks the <see cref="LogicalPair"/>.
        /// </summary>
        /// <param name="logicalPair">The <see cref="LogicalPair"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(LogicalPair logicalPair, ImmutableStack<string> path)
        {
            var condition = logicalPair.Condition;
            if (condition is object)
            {
                condition = (Sentence)this.Walk((Expression)logicalPair.Condition, path.Push(".Condition"));
            }

            var value = logicalPair.Value;
            if (value is object)
            {
                value = (Term)this.Walk((Expression)logicalPair.Value, path.Push(".Value"));
            }

            if (condition != logicalPair.Condition ||
                value != logicalPair.Value)
            {
                return new LogicalPair(
                    condition: condition,
                    value: value);
            }

            return logicalPair;
        }

        /// <summary>
        /// Walks the <see cref="LogicalSentence"/>.
        /// </summary>
        /// <param name="logicalSentence">The <see cref="LogicalSentence"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(LogicalSentence logicalSentence, ImmutableStack<string> path)
        {
            switch (logicalSentence)
            {
                case Conjunction conjunction:
                    logicalSentence = (LogicalSentence)this.Walk(conjunction, path);
                    break;
                case Disjunction disjunction:
                    logicalSentence = (LogicalSentence)this.Walk(disjunction, path);
                    break;
                case Equivalence equivalence:
                    logicalSentence = (LogicalSentence)this.Walk(equivalence, path);
                    break;
                case Implication implication:
                    logicalSentence = (LogicalSentence)this.Walk(implication, path);
                    break;
                case Negation negation:
                    logicalSentence = (LogicalSentence)this.Walk(negation, path);
                    break;
            }

            return logicalSentence;
        }

        /// <summary>
        /// Walks the <see cref="LogicalTerm"/>.
        /// </summary>
        /// <param name="logicalTerm">The <see cref="LogicalTerm"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(LogicalTerm logicalTerm, ImmutableStack<string> path)
        {
            switch (logicalTerm)
            {
                case ConditionalTerm conditionalTerm:
                    logicalTerm = (LogicalTerm)this.Walk(conditionalTerm, path);
                    break;
                case IfTerm ifTerm:
                    logicalTerm = (LogicalTerm)this.Walk(ifTerm, path);
                    break;
            }

            return logicalTerm;
        }

        /// <summary>
        /// Walks the <see cref="Negation"/>.
        /// </summary>
        /// <param name="negation">The <see cref="Negation"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Negation negation, ImmutableStack<string> path)
        {
            var negated = negation.Negated;
            if (negated is object)
            {
                negated = (Sentence)this.Walk((Expression)negation.Negated, path.Push(".Negated"));
            }

            if (negated != negation.Negated)
            {
                return new Negation(
                    negated: negated);
            }

            return negation;
        }

        /// <summary>
        /// Walks the <see cref="Operator"/>.
        /// </summary>
        /// <param name="operator">The <see cref="Operator"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Operator @operator, ImmutableStack<string> path)
        {
            return @operator;
        }

        /// <summary>
        /// Walks the <see cref="PartialDefinition"/>.
        /// </summary>
        /// <param name="partialDefinition">The <see cref="PartialDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(PartialDefinition partialDefinition, ImmutableStack<string> path)
        {
            switch (partialDefinition)
            {
                case PartialFunctionDefinition partialFunctionDefinition:
                    partialDefinition = (PartialDefinition)this.Walk(partialFunctionDefinition, path);
                    break;
                case PartialLogicalDefinition partialLogicalDefinition:
                    partialDefinition = (PartialDefinition)this.Walk(partialLogicalDefinition, path);
                    break;
                case PartialObjectDefinition partialObjectDefinition:
                    partialDefinition = (PartialDefinition)this.Walk(partialObjectDefinition, path);
                    break;
                case PartialRelationDefinition partialRelationDefinition:
                    partialDefinition = (PartialDefinition)this.Walk(partialRelationDefinition, path);
                    break;
                case ReversePartialFunctionDefinition reversePartialFunctionDefinition:
                    partialDefinition = (PartialDefinition)this.Walk(reversePartialFunctionDefinition, path);
                    break;
                case ReversePartialLogicalDefinition reversePartialLogicalDefinition:
                    partialDefinition = (PartialDefinition)this.Walk(reversePartialLogicalDefinition, path);
                    break;
                case ReversePartialObjectDefinition reversePartialObjectDefinition:
                    partialDefinition = (PartialDefinition)this.Walk(reversePartialObjectDefinition, path);
                    break;
                case ReversePartialRelationDefinition reversePartialRelationDefinition:
                    partialDefinition = (PartialDefinition)this.Walk(reversePartialRelationDefinition, path);
                    break;
            }

            return partialDefinition;
        }

        /// <summary>
        /// Walks the <see cref="PartialFunctionDefinition"/>.
        /// </summary>
        /// <param name="partialFunctionDefinition">The <see cref="PartialFunctionDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(PartialFunctionDefinition partialFunctionDefinition, ImmutableStack<string> path)
        {
            var constant = partialFunctionDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)partialFunctionDefinition.Constant, path.Push(".Constant"));
            }

            var description = partialFunctionDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)partialFunctionDefinition.Description, path.Push(".Description"));
            }

            var parameters = partialFunctionDefinition.Parameters;
            if (parameters is object)
            {
                for (var i = parameters.Count - 1; i >= 0; i--)
                {
                    var individualVariable = (IndividualVariable)this.Walk((Expression)parameters[i], path.Push($".Parameters[{i}]"));
                    if (individualVariable is null)
                    {
                        parameters = parameters.RemoveAt(i);
                    }
                    else if (individualVariable != parameters[i])
                    {
                        parameters = parameters.SetItem(i, individualVariable);
                    }
                }
            }

            var sequenceVariable = partialFunctionDefinition.SequenceVariable;
            if (sequenceVariable is object)
            {
                sequenceVariable = (SequenceVariable)this.Walk((Expression)partialFunctionDefinition.SequenceVariable, path.Push(".SequenceVariable"));
            }

            var variable = partialFunctionDefinition.Variable;
            if (variable is object)
            {
                variable = (IndividualVariable)this.Walk((Expression)partialFunctionDefinition.Variable, path.Push(".Variable"));
            }

            var sentence = partialFunctionDefinition.Sentence;
            if (sentence is object)
            {
                sentence = (Sentence)this.Walk((Expression)partialFunctionDefinition.Sentence, path.Push(".Sentence"));
            }

            if (constant != partialFunctionDefinition.Constant ||
                description != partialFunctionDefinition.Description ||
                parameters != partialFunctionDefinition.Parameters ||
                sequenceVariable != partialFunctionDefinition.SequenceVariable ||
                variable != partialFunctionDefinition.Variable ||
                sentence != partialFunctionDefinition.Sentence)
            {
                return new PartialFunctionDefinition(
                    constant: constant,
                    description: description,
                    parameters: parameters,
                    sequenceVariable: sequenceVariable,
                    variable: variable,
                    sentence: sentence);
            }

            return partialFunctionDefinition;
        }

        /// <summary>
        /// Walks the <see cref="PartialLogicalDefinition"/>.
        /// </summary>
        /// <param name="partialLogicalDefinition">The <see cref="PartialLogicalDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(PartialLogicalDefinition partialLogicalDefinition, ImmutableStack<string> path)
        {
            var constant = partialLogicalDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)partialLogicalDefinition.Constant, path.Push(".Constant"));
            }

            var description = partialLogicalDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)partialLogicalDefinition.Description, path.Push(".Description"));
            }

            var sentence = partialLogicalDefinition.Sentence;
            if (sentence is object)
            {
                sentence = (Sentence)this.Walk((Expression)partialLogicalDefinition.Sentence, path.Push(".Sentence"));
            }

            if (constant != partialLogicalDefinition.Constant ||
                description != partialLogicalDefinition.Description ||
                sentence != partialLogicalDefinition.Sentence)
            {
                return new PartialLogicalDefinition(
                    constant: constant,
                    description: description,
                    sentence: sentence);
            }

            return partialLogicalDefinition;
        }

        /// <summary>
        /// Walks the <see cref="PartialObjectDefinition"/>.
        /// </summary>
        /// <param name="partialObjectDefinition">The <see cref="PartialObjectDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(PartialObjectDefinition partialObjectDefinition, ImmutableStack<string> path)
        {
            var constant = partialObjectDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)partialObjectDefinition.Constant, path.Push(".Constant"));
            }

            var description = partialObjectDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)partialObjectDefinition.Description, path.Push(".Description"));
            }

            var variable = partialObjectDefinition.Variable;
            if (variable is object)
            {
                variable = (IndividualVariable)this.Walk((Expression)partialObjectDefinition.Variable, path.Push(".Variable"));
            }

            var sentence = partialObjectDefinition.Sentence;
            if (sentence is object)
            {
                sentence = (Sentence)this.Walk((Expression)partialObjectDefinition.Sentence, path.Push(".Sentence"));
            }

            if (constant != partialObjectDefinition.Constant ||
                description != partialObjectDefinition.Description ||
                variable != partialObjectDefinition.Variable ||
                sentence != partialObjectDefinition.Sentence)
            {
                return new PartialObjectDefinition(
                    constant: constant,
                    description: description,
                    variable: variable,
                    sentence: sentence);
            }

            return partialObjectDefinition;
        }

        /// <summary>
        /// Walks the <see cref="PartialRelationDefinition"/>.
        /// </summary>
        /// <param name="partialRelationDefinition">The <see cref="PartialRelationDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(PartialRelationDefinition partialRelationDefinition, ImmutableStack<string> path)
        {
            var constant = partialRelationDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)partialRelationDefinition.Constant, path.Push(".Constant"));
            }

            var description = partialRelationDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)partialRelationDefinition.Description, path.Push(".Description"));
            }

            var parameters = partialRelationDefinition.Parameters;
            if (parameters is object)
            {
                for (var i = parameters.Count - 1; i >= 0; i--)
                {
                    var individualVariable = (IndividualVariable)this.Walk((Expression)parameters[i], path.Push($".Parameters[{i}]"));
                    if (individualVariable is null)
                    {
                        parameters = parameters.RemoveAt(i);
                    }
                    else if (individualVariable != parameters[i])
                    {
                        parameters = parameters.SetItem(i, individualVariable);
                    }
                }
            }

            var sequenceVariable = partialRelationDefinition.SequenceVariable;
            if (sequenceVariable is object)
            {
                sequenceVariable = (SequenceVariable)this.Walk((Expression)partialRelationDefinition.SequenceVariable, path.Push(".SequenceVariable"));
            }

            var sentence = partialRelationDefinition.Sentence;
            if (sentence is object)
            {
                sentence = (Sentence)this.Walk((Expression)partialRelationDefinition.Sentence, path.Push(".Sentence"));
            }

            if (constant != partialRelationDefinition.Constant ||
                description != partialRelationDefinition.Description ||
                parameters != partialRelationDefinition.Parameters ||
                sequenceVariable != partialRelationDefinition.SequenceVariable ||
                sentence != partialRelationDefinition.Sentence)
            {
                return new PartialRelationDefinition(
                    constant: constant,
                    description: description,
                    parameters: parameters,
                    sequenceVariable: sequenceVariable,
                    sentence: sentence);
            }

            return partialRelationDefinition;
        }

        /// <summary>
        /// Walks the <see cref="QuantifiedSentence"/>.
        /// </summary>
        /// <param name="quantifiedSentence">The <see cref="QuantifiedSentence"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(QuantifiedSentence quantifiedSentence, ImmutableStack<string> path)
        {
            switch (quantifiedSentence)
            {
                case ExistentiallyQuantifiedSentence existentiallyQuantifiedSentence:
                    quantifiedSentence = (QuantifiedSentence)this.Walk(existentiallyQuantifiedSentence, path);
                    break;
                case UniversallyQuantifiedSentence universallyQuantifiedSentence:
                    quantifiedSentence = (QuantifiedSentence)this.Walk(universallyQuantifiedSentence, path);
                    break;
            }

            return quantifiedSentence;
        }

        /// <summary>
        /// Walks the <see cref="Quotation"/>.
        /// </summary>
        /// <param name="quotation">The <see cref="Quotation"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Quotation quotation, ImmutableStack<string> path)
        {
            var quoted = quotation.Quoted;
            if (quoted is object)
            {
                quoted = this.Walk(quotation.Quoted, path.Push(".Quoted"));
            }

            if (quoted != quotation.Quoted)
            {
                return new Quotation(
                    quoted: quoted);
            }

            return quotation;
        }

        /// <summary>
        /// Walks the <see cref="RelationalSentence"/>.
        /// </summary>
        /// <param name="relationalSentence">The <see cref="RelationalSentence"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(RelationalSentence relationalSentence, ImmutableStack<string> path)
        {
            switch (relationalSentence)
            {
                case ExplicitRelationalSentence explicitRelationalSentence:
                    relationalSentence = (RelationalSentence)this.Walk(explicitRelationalSentence, path);
                    break;
                case ImplicitRelationalSentence implicitRelationalSentence:
                    relationalSentence = (RelationalSentence)this.Walk(implicitRelationalSentence, path);
                    break;
            }

            return relationalSentence;
        }

        /// <summary>
        /// Walks the <see cref="ReverseImplication"/>.
        /// </summary>
        /// <param name="reverseImplication">The <see cref="ReverseImplication"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ReverseImplication reverseImplication, ImmutableStack<string> path)
        {
            var consequent = reverseImplication.Consequent;
            if (consequent is object)
            {
                consequent = (Sentence)this.Walk((Expression)reverseImplication.Consequent, path.Push(".Consequent"));
            }

            var antecedents = reverseImplication.Antecedents;
            if (antecedents is object)
            {
                for (var i = antecedents.Count - 1; i >= 0; i--)
                {
                    var sentence = (Sentence)this.Walk((Expression)antecedents[i], path.Push($".Antecedents[{i}]"));
                    if (sentence is null)
                    {
                        antecedents = antecedents.RemoveAt(i);
                    }
                    else if (sentence != antecedents[i])
                    {
                        antecedents = antecedents.SetItem(i, sentence);
                    }
                }
            }

            if (consequent != reverseImplication.Consequent ||
                antecedents != reverseImplication.Antecedents)
            {
                return new ReverseImplication(
                    consequent: consequent,
                    antecedents: antecedents);
            }

            return reverseImplication;
        }

        /// <summary>
        /// Walks the <see cref="ReversePartialFunctionDefinition"/>.
        /// </summary>
        /// <param name="reversePartialFunctionDefinition">The <see cref="ReversePartialFunctionDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ReversePartialFunctionDefinition reversePartialFunctionDefinition, ImmutableStack<string> path)
        {
            var constant = reversePartialFunctionDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)reversePartialFunctionDefinition.Constant, path.Push(".Constant"));
            }

            var description = reversePartialFunctionDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)reversePartialFunctionDefinition.Description, path.Push(".Description"));
            }

            var parameters = reversePartialFunctionDefinition.Parameters;
            if (parameters is object)
            {
                for (var i = parameters.Count - 1; i >= 0; i--)
                {
                    var individualVariable = (IndividualVariable)this.Walk((Expression)parameters[i], path.Push($".Parameters[{i}]"));
                    if (individualVariable is null)
                    {
                        parameters = parameters.RemoveAt(i);
                    }
                    else if (individualVariable != parameters[i])
                    {
                        parameters = parameters.SetItem(i, individualVariable);
                    }
                }
            }

            var sequenceVariable = reversePartialFunctionDefinition.SequenceVariable;
            if (sequenceVariable is object)
            {
                sequenceVariable = (SequenceVariable)this.Walk((Expression)reversePartialFunctionDefinition.SequenceVariable, path.Push(".SequenceVariable"));
            }

            var variable = reversePartialFunctionDefinition.Variable;
            if (variable is object)
            {
                variable = (IndividualVariable)this.Walk((Expression)reversePartialFunctionDefinition.Variable, path.Push(".Variable"));
            }

            var sentence = reversePartialFunctionDefinition.Sentence;
            if (sentence is object)
            {
                sentence = (Sentence)this.Walk((Expression)reversePartialFunctionDefinition.Sentence, path.Push(".Sentence"));
            }

            if (constant != reversePartialFunctionDefinition.Constant ||
                description != reversePartialFunctionDefinition.Description ||
                parameters != reversePartialFunctionDefinition.Parameters ||
                sequenceVariable != reversePartialFunctionDefinition.SequenceVariable ||
                variable != reversePartialFunctionDefinition.Variable ||
                sentence != reversePartialFunctionDefinition.Sentence)
            {
                return new ReversePartialFunctionDefinition(
                    constant: constant,
                    description: description,
                    parameters: parameters,
                    sequenceVariable: sequenceVariable,
                    variable: variable,
                    sentence: sentence);
            }

            return reversePartialFunctionDefinition;
        }

        /// <summary>
        /// Walks the <see cref="ReversePartialLogicalDefinition"/>.
        /// </summary>
        /// <param name="reversePartialLogicalDefinition">The <see cref="ReversePartialLogicalDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ReversePartialLogicalDefinition reversePartialLogicalDefinition, ImmutableStack<string> path)
        {
            var constant = reversePartialLogicalDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)reversePartialLogicalDefinition.Constant, path.Push(".Constant"));
            }

            var description = reversePartialLogicalDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)reversePartialLogicalDefinition.Description, path.Push(".Description"));
            }

            var sentence = reversePartialLogicalDefinition.Sentence;
            if (sentence is object)
            {
                sentence = (Sentence)this.Walk((Expression)reversePartialLogicalDefinition.Sentence, path.Push(".Sentence"));
            }

            if (constant != reversePartialLogicalDefinition.Constant ||
                description != reversePartialLogicalDefinition.Description ||
                sentence != reversePartialLogicalDefinition.Sentence)
            {
                return new ReversePartialLogicalDefinition(
                    constant: constant,
                    description: description,
                    sentence: sentence);
            }

            return reversePartialLogicalDefinition;
        }

        /// <summary>
        /// Walks the <see cref="ReversePartialObjectDefinition"/>.
        /// </summary>
        /// <param name="reversePartialObjectDefinition">The <see cref="ReversePartialObjectDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ReversePartialObjectDefinition reversePartialObjectDefinition, ImmutableStack<string> path)
        {
            var constant = reversePartialObjectDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)reversePartialObjectDefinition.Constant, path.Push(".Constant"));
            }

            var description = reversePartialObjectDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)reversePartialObjectDefinition.Description, path.Push(".Description"));
            }

            var variable = reversePartialObjectDefinition.Variable;
            if (variable is object)
            {
                variable = (IndividualVariable)this.Walk((Expression)reversePartialObjectDefinition.Variable, path.Push(".Variable"));
            }

            var sentence = reversePartialObjectDefinition.Sentence;
            if (sentence is object)
            {
                sentence = (Sentence)this.Walk((Expression)reversePartialObjectDefinition.Sentence, path.Push(".Sentence"));
            }

            if (constant != reversePartialObjectDefinition.Constant ||
                description != reversePartialObjectDefinition.Description ||
                variable != reversePartialObjectDefinition.Variable ||
                sentence != reversePartialObjectDefinition.Sentence)
            {
                return new ReversePartialObjectDefinition(
                    constant: constant,
                    description: description,
                    variable: variable,
                    sentence: sentence);
            }

            return reversePartialObjectDefinition;
        }

        /// <summary>
        /// Walks the <see cref="ReversePartialRelationDefinition"/>.
        /// </summary>
        /// <param name="reversePartialRelationDefinition">The <see cref="ReversePartialRelationDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(ReversePartialRelationDefinition reversePartialRelationDefinition, ImmutableStack<string> path)
        {
            var constant = reversePartialRelationDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)reversePartialRelationDefinition.Constant, path.Push(".Constant"));
            }

            var description = reversePartialRelationDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)reversePartialRelationDefinition.Description, path.Push(".Description"));
            }

            var parameters = reversePartialRelationDefinition.Parameters;
            if (parameters is object)
            {
                for (var i = parameters.Count - 1; i >= 0; i--)
                {
                    var individualVariable = (IndividualVariable)this.Walk((Expression)parameters[i], path.Push($".Parameters[{i}]"));
                    if (individualVariable is null)
                    {
                        parameters = parameters.RemoveAt(i);
                    }
                    else if (individualVariable != parameters[i])
                    {
                        parameters = parameters.SetItem(i, individualVariable);
                    }
                }
            }

            var sequenceVariable = reversePartialRelationDefinition.SequenceVariable;
            if (sequenceVariable is object)
            {
                sequenceVariable = (SequenceVariable)this.Walk((Expression)reversePartialRelationDefinition.SequenceVariable, path.Push(".SequenceVariable"));
            }

            var sentence = reversePartialRelationDefinition.Sentence;
            if (sentence is object)
            {
                sentence = (Sentence)this.Walk((Expression)reversePartialRelationDefinition.Sentence, path.Push(".Sentence"));
            }

            if (constant != reversePartialRelationDefinition.Constant ||
                description != reversePartialRelationDefinition.Description ||
                parameters != reversePartialRelationDefinition.Parameters ||
                sequenceVariable != reversePartialRelationDefinition.SequenceVariable ||
                sentence != reversePartialRelationDefinition.Sentence)
            {
                return new ReversePartialRelationDefinition(
                    constant: constant,
                    description: description,
                    parameters: parameters,
                    sequenceVariable: sequenceVariable,
                    sentence: sentence);
            }

            return reversePartialRelationDefinition;
        }

        /// <summary>
        /// Walks the <see cref="Sentence"/>.
        /// </summary>
        /// <param name="sentence">The <see cref="Sentence"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Sentence sentence, ImmutableStack<string> path)
        {
            switch (sentence)
            {
                case ConstantSentence constantSentence:
                    sentence = (Sentence)this.Walk(constantSentence, path);
                    break;
                case Equation equation:
                    sentence = (Sentence)this.Walk(equation, path);
                    break;
                case Inequality inequality:
                    sentence = (Sentence)this.Walk(inequality, path);
                    break;
                case LogicalSentence logicalSentence:
                    sentence = (Sentence)this.Walk(logicalSentence, path);
                    break;
                case QuantifiedSentence quantifiedSentence:
                    sentence = (Sentence)this.Walk(quantifiedSentence, path);
                    break;
                case RelationalSentence relationalSentence:
                    sentence = (Sentence)this.Walk(relationalSentence, path);
                    break;
            }

            return sentence;
        }

        /// <summary>
        /// Walks the <see cref="SequenceVariable"/>.
        /// </summary>
        /// <param name="sequenceVariable">The <see cref="SequenceVariable"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(SequenceVariable sequenceVariable, ImmutableStack<string> path)
        {
            return sequenceVariable;
        }

        /// <summary>
        /// Walks the <see cref="Term"/>.
        /// </summary>
        /// <param name="term">The <see cref="Term"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Term term, ImmutableStack<string> path)
        {
            switch (term)
            {
                case CharacterReference characterReference:
                    term = (Term)this.Walk(characterReference, path);
                    break;
                case FunctionalTerm functionalTerm:
                    term = (Term)this.Walk(functionalTerm, path);
                    break;
                case ListTerm listTerm:
                    term = (Term)this.Walk(listTerm, path);
                    break;
                case LogicalTerm logicalTerm:
                    term = (Term)this.Walk(logicalTerm, path);
                    break;
                case Quotation quotation:
                    term = (Term)this.Walk(quotation, path);
                    break;
                case WordTerm wordTerm:
                    term = (Term)this.Walk(wordTerm, path);
                    break;
            }

            return term;
        }

        /// <summary>
        /// Walks the <see cref="UniversallyQuantifiedSentence"/>.
        /// </summary>
        /// <param name="universallyQuantifiedSentence">The <see cref="UniversallyQuantifiedSentence"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(UniversallyQuantifiedSentence universallyQuantifiedSentence, ImmutableStack<string> path)
        {
            var variables = universallyQuantifiedSentence.Variables;
            if (variables is object)
            {
                for (var i = variables.Count - 1; i >= 0; i--)
                {
                    var variableSpecification = (VariableSpecification)this.Walk((Expression)variables[i], path.Push($".Variables[{i}]"));
                    if (variableSpecification is null)
                    {
                        variables = variables.RemoveAt(i);
                    }
                    else if (variableSpecification != variables[i])
                    {
                        variables = variables.SetItem(i, variableSpecification);
                    }
                }
            }

            var quantified = universallyQuantifiedSentence.Quantified;
            if (quantified is object)
            {
                quantified = (Sentence)this.Walk((Expression)universallyQuantifiedSentence.Quantified, path.Push(".Quantified"));
            }

            if (variables != universallyQuantifiedSentence.Variables ||
                quantified != universallyQuantifiedSentence.Quantified)
            {
                return new UniversallyQuantifiedSentence(
                    variables: variables,
                    quantified: quantified);
            }

            return universallyQuantifiedSentence;
        }

        /// <summary>
        /// Walks the <see cref="UnrestrictedDefinition"/>.
        /// </summary>
        /// <param name="unrestrictedDefinition">The <see cref="UnrestrictedDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(UnrestrictedDefinition unrestrictedDefinition, ImmutableStack<string> path)
        {
            switch (unrestrictedDefinition)
            {
                case UnrestrictedFunctionDefinition unrestrictedFunctionDefinition:
                    unrestrictedDefinition = (UnrestrictedDefinition)this.Walk(unrestrictedFunctionDefinition, path);
                    break;
                case UnrestrictedLogicalDefinition unrestrictedLogicalDefinition:
                    unrestrictedDefinition = (UnrestrictedDefinition)this.Walk(unrestrictedLogicalDefinition, path);
                    break;
                case UnrestrictedObjectDefinition unrestrictedObjectDefinition:
                    unrestrictedDefinition = (UnrestrictedDefinition)this.Walk(unrestrictedObjectDefinition, path);
                    break;
                case UnrestrictedRelationDefinition unrestrictedRelationDefinition:
                    unrestrictedDefinition = (UnrestrictedDefinition)this.Walk(unrestrictedRelationDefinition, path);
                    break;
            }

            return unrestrictedDefinition;
        }

        /// <summary>
        /// Walks the <see cref="UnrestrictedFunctionDefinition"/>.
        /// </summary>
        /// <param name="unrestrictedFunctionDefinition">The <see cref="UnrestrictedFunctionDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(UnrestrictedFunctionDefinition unrestrictedFunctionDefinition, ImmutableStack<string> path)
        {
            var constant = unrestrictedFunctionDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)unrestrictedFunctionDefinition.Constant, path.Push(".Constant"));
            }

            var description = unrestrictedFunctionDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)unrestrictedFunctionDefinition.Description, path.Push(".Description"));
            }

            var sentences = unrestrictedFunctionDefinition.Sentences;
            if (sentences is object)
            {
                for (var i = sentences.Count - 1; i >= 0; i--)
                {
                    var sentence = (Sentence)this.Walk((Expression)sentences[i], path.Push($".Sentences[{i}]"));
                    if (sentence is null)
                    {
                        sentences = sentences.RemoveAt(i);
                    }
                    else if (sentence != sentences[i])
                    {
                        sentences = sentences.SetItem(i, sentence);
                    }
                }
            }

            if (constant != unrestrictedFunctionDefinition.Constant ||
                description != unrestrictedFunctionDefinition.Description ||
                sentences != unrestrictedFunctionDefinition.Sentences)
            {
                return new UnrestrictedFunctionDefinition(
                    constant: constant,
                    description: description,
                    sentences: sentences);
            }

            return unrestrictedFunctionDefinition;
        }

        /// <summary>
        /// Walks the <see cref="UnrestrictedLogicalDefinition"/>.
        /// </summary>
        /// <param name="unrestrictedLogicalDefinition">The <see cref="UnrestrictedLogicalDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(UnrestrictedLogicalDefinition unrestrictedLogicalDefinition, ImmutableStack<string> path)
        {
            var constant = unrestrictedLogicalDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)unrestrictedLogicalDefinition.Constant, path.Push(".Constant"));
            }

            var description = unrestrictedLogicalDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)unrestrictedLogicalDefinition.Description, path.Push(".Description"));
            }

            var sentences = unrestrictedLogicalDefinition.Sentences;
            if (sentences is object)
            {
                for (var i = sentences.Count - 1; i >= 0; i--)
                {
                    var sentence = (Sentence)this.Walk((Expression)sentences[i], path.Push($".Sentences[{i}]"));
                    if (sentence is null)
                    {
                        sentences = sentences.RemoveAt(i);
                    }
                    else if (sentence != sentences[i])
                    {
                        sentences = sentences.SetItem(i, sentence);
                    }
                }
            }

            if (constant != unrestrictedLogicalDefinition.Constant ||
                description != unrestrictedLogicalDefinition.Description ||
                sentences != unrestrictedLogicalDefinition.Sentences)
            {
                return new UnrestrictedLogicalDefinition(
                    constant: constant,
                    description: description,
                    sentences: sentences);
            }

            return unrestrictedLogicalDefinition;
        }

        /// <summary>
        /// Walks the <see cref="UnrestrictedObjectDefinition"/>.
        /// </summary>
        /// <param name="unrestrictedObjectDefinition">The <see cref="UnrestrictedObjectDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(UnrestrictedObjectDefinition unrestrictedObjectDefinition, ImmutableStack<string> path)
        {
            var constant = unrestrictedObjectDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)unrestrictedObjectDefinition.Constant, path.Push(".Constant"));
            }

            var description = unrestrictedObjectDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)unrestrictedObjectDefinition.Description, path.Push(".Description"));
            }

            var sentences = unrestrictedObjectDefinition.Sentences;
            if (sentences is object)
            {
                for (var i = sentences.Count - 1; i >= 0; i--)
                {
                    var sentence = (Sentence)this.Walk((Expression)sentences[i], path.Push($".Sentences[{i}]"));
                    if (sentence is null)
                    {
                        sentences = sentences.RemoveAt(i);
                    }
                    else if (sentence != sentences[i])
                    {
                        sentences = sentences.SetItem(i, sentence);
                    }
                }
            }

            if (constant != unrestrictedObjectDefinition.Constant ||
                description != unrestrictedObjectDefinition.Description ||
                sentences != unrestrictedObjectDefinition.Sentences)
            {
                return new UnrestrictedObjectDefinition(
                    constant: constant,
                    description: description,
                    sentences: sentences);
            }

            return unrestrictedObjectDefinition;
        }

        /// <summary>
        /// Walks the <see cref="UnrestrictedRelationDefinition"/>.
        /// </summary>
        /// <param name="unrestrictedRelationDefinition">The <see cref="UnrestrictedRelationDefinition"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(UnrestrictedRelationDefinition unrestrictedRelationDefinition, ImmutableStack<string> path)
        {
            var constant = unrestrictedRelationDefinition.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)unrestrictedRelationDefinition.Constant, path.Push(".Constant"));
            }

            var description = unrestrictedRelationDefinition.Description;
            if (description is object)
            {
                description = (CharacterString)this.Walk((Expression)unrestrictedRelationDefinition.Description, path.Push(".Description"));
            }

            var sentences = unrestrictedRelationDefinition.Sentences;
            if (sentences is object)
            {
                for (var i = sentences.Count - 1; i >= 0; i--)
                {
                    var sentence = (Sentence)this.Walk((Expression)sentences[i], path.Push($".Sentences[{i}]"));
                    if (sentence is null)
                    {
                        sentences = sentences.RemoveAt(i);
                    }
                    else if (sentence != sentences[i])
                    {
                        sentences = sentences.SetItem(i, sentence);
                    }
                }
            }

            if (constant != unrestrictedRelationDefinition.Constant ||
                description != unrestrictedRelationDefinition.Description ||
                sentences != unrestrictedRelationDefinition.Sentences)
            {
                return new UnrestrictedRelationDefinition(
                    constant: constant,
                    description: description,
                    sentences: sentences);
            }

            return unrestrictedRelationDefinition;
        }

        /// <summary>
        /// Walks the <see cref="Variable"/>.
        /// </summary>
        /// <param name="variable">The <see cref="Variable"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(Variable variable, ImmutableStack<string> path)
        {
            switch (variable)
            {
                case IndividualVariable individualVariable:
                    variable = (Variable)this.Walk(individualVariable, path);
                    break;
                case SequenceVariable sequenceVariable:
                    variable = (Variable)this.Walk(sequenceVariable, path);
                    break;
            }

            return variable;
        }

        /// <summary>
        /// Walks the <see cref="VariableSpecification"/>.
        /// </summary>
        /// <param name="variableSpecification">The <see cref="VariableSpecification"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(VariableSpecification variableSpecification, ImmutableStack<string> path)
        {
            var variable = variableSpecification.Variable;
            if (variable is object)
            {
                variable = (Variable)this.Walk((Expression)variableSpecification.Variable, path.Push(".Variable"));
            }

            var constant = variableSpecification.Constant;
            if (constant is object)
            {
                constant = (Constant)this.Walk((Expression)variableSpecification.Constant, path.Push(".Constant"));
            }

            if (variable != variableSpecification.Variable ||
                constant != variableSpecification.Constant)
            {
                return new VariableSpecification(
                    variable: variable,
                    constant: constant);
            }

            return variableSpecification;
        }

        /// <summary>
        /// Walks the <see cref="WordTerm"/>.
        /// </summary>
        /// <param name="wordTerm">The <see cref="WordTerm"/> to walk.</param>
        /// <param name="path">The path of the current node in the tree, expressed as a stack of JSON Path expressions.</param>
        /// <returns>A replacement expression.</returns>
        public virtual Expression Walk(WordTerm wordTerm, ImmutableStack<string> path)
        {
            switch (wordTerm)
            {
                case Constant constant:
                    wordTerm = (WordTerm)this.Walk(constant, path);
                    break;
                case Operator @operator:
                    wordTerm = (WordTerm)this.Walk(@operator, path);
                    break;
                case Variable variable:
                    wordTerm = (WordTerm)this.Walk(variable, path);
                    break;
            }

            return wordTerm;
        }
    }
}
