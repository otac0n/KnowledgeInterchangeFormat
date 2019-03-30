// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat
{
    using KnowledgeInterchangeFormat.Expressions;

    /*
     * Generated with the following script:
     *
        var assembly = Assembly.LoadFrom(path);
        var expressionType = assembly.GetType("KnowledgeInterchangeFormat.Expressions.Expression");
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(expressionType);
        string typeName(Type t) => t.Name;
        var types = assembly.GetTypes().OrderBy(typeName).Where(t => expressionType.IsAssignableFrom(t)).ToList();
        string e(string name) => name == "operator" ? "@" + name : name;
        string lowerFirst(string name) => e(char.ToLowerInvariant(name[0]) + name.Substring(1));

        var descendents = types.ToLookup(t => t.BaseType);
        foreach (var type in types)
        {
            var name = type.Name;
            $"/// <summary>".Dump();
            $"/// Walks the <see cref=\"{name}\"/>.".Dump();
            $"/// </summary>".Dump();
            $"/// <param name=\"{lowerFirst(name).TrimStart('@')}\">The <see cref=\"{name}\"/> to walk.</param>".Dump();
            $"public virtual void Walk({name} {lowerFirst(name)})".Dump();
            "{".Dump();
            if (type.IsAbstract)
            {
                $"    switch ({lowerFirst(name)})".Dump();
                "    {".Dump();
                foreach (var descendent in descendents[type].OrderBy(typeName))
                {
                    $"        case {descendent.Name} {lowerFirst(descendent.Name)}:".Dump();
                    $"            this.Walk({lowerFirst(descendent.Name)});".Dump();
                    "            break;".Dump();
                }
                "    }".Dump();
            }
            else if (type.Name == "CharacterString" || type.Name == "CharacterBlock")
            {
                // These types are special cases of Lists that can be considered atomic.
            }
            else
            {
                var constructorParams = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).First().GetParameters().Select(p => p.Name.ToUpperInvariant()).ToList();
                var first = true;
                foreach (var property in type.GetProperties().Where(p => p.DeclaringType == type || !type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).Any(d => d.Name == p.Name)).OrderBy(p => (uint)constructorParams.IndexOf(p.Name.ToUpperInvariant())))
                {
                    if (!first) "".Dump();
                    if (expressionType.IsAssignableFrom(property.PropertyType))
                    {
                        first = false;
                        $"    if ({lowerFirst(name)}.{property.Name} != null)".Dump();
                        "    {".Dump();
                        $"        this.Walk({(property.PropertyType == expressionType ? "" : "(Expression)")}{lowerFirst(name)}.{property.Name});".Dump();
                        "    }".Dump();
                    }
                    else if (enumerableType.IsAssignableFrom(property.PropertyType))
                    {
                        first = false;
                        var subType = property.PropertyType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)).Single().GenericTypeArguments.Single();
                        var subTypeName = subType == type ? "sub" + type.Name : lowerFirst(subType.Name);
                        $"    if ({lowerFirst(name)}.{property.Name} != null)".Dump();
                        "    {".Dump();
                        $"        foreach(var {subTypeName} in {lowerFirst(name)}.{property.Name})".Dump();
                        "        {".Dump();
                        $"            this.Walk({(subType == expressionType ? "" : "(Expression)")}{subTypeName});".Dump();
                        "        }".Dump();
                        "    }".Dump();
                    }
                }
            }
            "}".Dump();
            "".Dump();
        }
     */

    /// <summary>
    /// A base class for walking expression trees.
    /// </summary>
    public class ExpressionTreeWalker
    {
        /// <summary>
        /// Walks the <see cref="CharacterBlock"/>.
        /// </summary>
        /// <param name="characterBlock">The <see cref="CharacterBlock"/> to walk.</param>
        public virtual void Walk(CharacterBlock characterBlock)
        {
        }

        /// <summary>
        /// Walks the <see cref="CharacterReference"/>.
        /// </summary>
        /// <param name="characterReference">The <see cref="CharacterReference"/> to walk.</param>
        public virtual void Walk(CharacterReference characterReference)
        {
        }

        /// <summary>
        /// Walks the <see cref="CharacterString"/>.
        /// </summary>
        /// <param name="characterString">The <see cref="CharacterString"/> to walk.</param>
        public virtual void Walk(CharacterString characterString)
        {
        }

        /// <summary>
        /// Walks the <see cref="CompleteDefinition"/>.
        /// </summary>
        /// <param name="completeDefinition">The <see cref="CompleteDefinition"/> to walk.</param>
        public virtual void Walk(CompleteDefinition completeDefinition)
        {
            switch (completeDefinition)
            {
                case CompleteFunctionDefinition completeFunctionDefinition:
                    this.Walk(completeFunctionDefinition);
                    break;
                case CompleteLogicalDefinition completeLogicalDefinition:
                    this.Walk(completeLogicalDefinition);
                    break;
                case CompleteObjectDefinition completeObjectDefinition:
                    this.Walk(completeObjectDefinition);
                    break;
                case CompleteRelationDefinition completeRelationDefinition:
                    this.Walk(completeRelationDefinition);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="CompleteFunctionDefinition"/>.
        /// </summary>
        /// <param name="completeFunctionDefinition">The <see cref="CompleteFunctionDefinition"/> to walk.</param>
        public virtual void Walk(CompleteFunctionDefinition completeFunctionDefinition)
        {
            if (completeFunctionDefinition.Constant != null)
            {
                this.Walk((Expression)completeFunctionDefinition.Constant);
            }

            if (completeFunctionDefinition.Description != null)
            {
                this.Walk((Expression)completeFunctionDefinition.Description);
            }

            if (completeFunctionDefinition.Parameters != null)
            {
                foreach (var individualVariable in completeFunctionDefinition.Parameters)
                {
                    this.Walk((Expression)individualVariable);
                }
            }

            if (completeFunctionDefinition.SequenceVariable != null)
            {
                this.Walk((Expression)completeFunctionDefinition.SequenceVariable);
            }

            if (completeFunctionDefinition.Term != null)
            {
                this.Walk((Expression)completeFunctionDefinition.Term);
            }
        }

        /// <summary>
        /// Walks the <see cref="CompleteLogicalDefinition"/>.
        /// </summary>
        /// <param name="completeLogicalDefinition">The <see cref="CompleteLogicalDefinition"/> to walk.</param>
        public virtual void Walk(CompleteLogicalDefinition completeLogicalDefinition)
        {
            if (completeLogicalDefinition.Constant != null)
            {
                this.Walk((Expression)completeLogicalDefinition.Constant);
            }

            if (completeLogicalDefinition.Description != null)
            {
                this.Walk((Expression)completeLogicalDefinition.Description);
            }

            if (completeLogicalDefinition.Sentence != null)
            {
                this.Walk((Expression)completeLogicalDefinition.Sentence);
            }
        }

        /// <summary>
        /// Walks the <see cref="CompleteObjectDefinition"/>.
        /// </summary>
        /// <param name="completeObjectDefinition">The <see cref="CompleteObjectDefinition"/> to walk.</param>
        public virtual void Walk(CompleteObjectDefinition completeObjectDefinition)
        {
            if (completeObjectDefinition.Constant != null)
            {
                this.Walk((Expression)completeObjectDefinition.Constant);
            }

            if (completeObjectDefinition.Description != null)
            {
                this.Walk((Expression)completeObjectDefinition.Description);
            }

            if (completeObjectDefinition.Term != null)
            {
                this.Walk((Expression)completeObjectDefinition.Term);
            }
        }

        /// <summary>
        /// Walks the <see cref="CompleteRelationDefinition"/>.
        /// </summary>
        /// <param name="completeRelationDefinition">The <see cref="CompleteRelationDefinition"/> to walk.</param>
        public virtual void Walk(CompleteRelationDefinition completeRelationDefinition)
        {
            if (completeRelationDefinition.Constant != null)
            {
                this.Walk((Expression)completeRelationDefinition.Constant);
            }

            if (completeRelationDefinition.Description != null)
            {
                this.Walk((Expression)completeRelationDefinition.Description);
            }

            if (completeRelationDefinition.Parameters != null)
            {
                foreach (var individualVariable in completeRelationDefinition.Parameters)
                {
                    this.Walk((Expression)individualVariable);
                }
            }

            if (completeRelationDefinition.SequenceVariable != null)
            {
                this.Walk((Expression)completeRelationDefinition.SequenceVariable);
            }

            if (completeRelationDefinition.Sentence != null)
            {
                this.Walk((Expression)completeRelationDefinition.Sentence);
            }
        }

        /// <summary>
        /// Walks the <see cref="ConditionalTerm"/>.
        /// </summary>
        /// <param name="conditionalTerm">The <see cref="ConditionalTerm"/> to walk.</param>
        public virtual void Walk(ConditionalTerm conditionalTerm)
        {
            if (conditionalTerm.Pairs != null)
            {
                foreach (var logicalPair in conditionalTerm.Pairs)
                {
                    this.Walk((Expression)logicalPair);
                }
            }
        }

        /// <summary>
        /// Walks the <see cref="Conjunction"/>.
        /// </summary>
        /// <param name="conjunction">The <see cref="Conjunction"/> to walk.</param>
        public virtual void Walk(Conjunction conjunction)
        {
            if (conjunction.Conjuncts != null)
            {
                foreach (var sentence in conjunction.Conjuncts)
                {
                    this.Walk((Expression)sentence);
                }
            }
        }

        /// <summary>
        /// Walks the <see cref="Constant"/>.
        /// </summary>
        /// <param name="constant">The <see cref="Constant"/> to walk.</param>
        public virtual void Walk(Constant constant)
        {
        }

        /// <summary>
        /// Walks the <see cref="ConstantSentence"/>.
        /// </summary>
        /// <param name="constantSentence">The <see cref="ConstantSentence"/> to walk.</param>
        public virtual void Walk(ConstantSentence constantSentence)
        {
            if (constantSentence.Constant != null)
            {
                this.Walk((Expression)constantSentence.Constant);
            }
        }

        /// <summary>
        /// Walks the <see cref="Definition"/>.
        /// </summary>
        /// <param name="definition">The <see cref="Definition"/> to walk.</param>
        public virtual void Walk(Definition definition)
        {
            switch (definition)
            {
                case CompleteDefinition completeDefinition:
                    this.Walk(completeDefinition);
                    break;
                case PartialDefinition partialDefinition:
                    this.Walk(partialDefinition);
                    break;
                case UnrestrictedDefinition unrestrictedDefinition:
                    this.Walk(unrestrictedDefinition);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="Disjunction"/>.
        /// </summary>
        /// <param name="disjunction">The <see cref="Disjunction"/> to walk.</param>
        public virtual void Walk(Disjunction disjunction)
        {
            if (disjunction.Disjuncts != null)
            {
                foreach (var sentence in disjunction.Disjuncts)
                {
                    this.Walk((Expression)sentence);
                }
            }
        }

        /// <summary>
        /// Walks the <see cref="Equation"/>.
        /// </summary>
        /// <param name="equation">The <see cref="Equation"/> to walk.</param>
        public virtual void Walk(Equation equation)
        {
            if (equation.Left != null)
            {
                this.Walk((Expression)equation.Left);
            }

            if (equation.Right != null)
            {
                this.Walk((Expression)equation.Right);
            }
        }

        /// <summary>
        /// Walks the <see cref="Equivalence"/>.
        /// </summary>
        /// <param name="equivalence">The <see cref="Equivalence"/> to walk.</param>
        public virtual void Walk(Equivalence equivalence)
        {
            if (equivalence.Left != null)
            {
                this.Walk((Expression)equivalence.Left);
            }

            if (equivalence.Right != null)
            {
                this.Walk((Expression)equivalence.Right);
            }
        }

        /// <summary>
        /// Walks the <see cref="ExistentiallyQuantifiedSentence"/>.
        /// </summary>
        /// <param name="existentiallyQuantifiedSentence">The <see cref="ExistentiallyQuantifiedSentence"/> to walk.</param>
        public virtual void Walk(ExistentiallyQuantifiedSentence existentiallyQuantifiedSentence)
        {
            if (existentiallyQuantifiedSentence.Variables != null)
            {
                foreach (var variableSpecification in existentiallyQuantifiedSentence.Variables)
                {
                    this.Walk((Expression)variableSpecification);
                }
            }

            if (existentiallyQuantifiedSentence.Quantified != null)
            {
                this.Walk((Expression)existentiallyQuantifiedSentence.Quantified);
            }
        }

        /// <summary>
        /// Walks the <see cref="ExplicitFunctionalTerm"/>.
        /// </summary>
        /// <param name="explicitFunctionalTerm">The <see cref="ExplicitFunctionalTerm"/> to walk.</param>
        public virtual void Walk(ExplicitFunctionalTerm explicitFunctionalTerm)
        {
            if (explicitFunctionalTerm.Function != null)
            {
                this.Walk((Expression)explicitFunctionalTerm.Function);
            }

            if (explicitFunctionalTerm.Arguments != null)
            {
                foreach (var term in explicitFunctionalTerm.Arguments)
                {
                    this.Walk((Expression)term);
                }
            }

            if (explicitFunctionalTerm.SequenceVariable != null)
            {
                this.Walk((Expression)explicitFunctionalTerm.SequenceVariable);
            }
        }

        /// <summary>
        /// Walks the <see cref="ExplicitRelationalSentence"/>.
        /// </summary>
        /// <param name="explicitRelationalSentence">The <see cref="ExplicitRelationalSentence"/> to walk.</param>
        public virtual void Walk(ExplicitRelationalSentence explicitRelationalSentence)
        {
            if (explicitRelationalSentence.Relation != null)
            {
                this.Walk((Expression)explicitRelationalSentence.Relation);
            }

            if (explicitRelationalSentence.Arguments != null)
            {
                foreach (var term in explicitRelationalSentence.Arguments)
                {
                    this.Walk((Expression)term);
                }
            }

            if (explicitRelationalSentence.SequenceVariable != null)
            {
                this.Walk((Expression)explicitRelationalSentence.SequenceVariable);
            }
        }

        /// <summary>
        /// Walks the <see cref="Expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> to walk.</param>
        public virtual void Walk(Expression expression)
        {
            switch (expression)
            {
                case Form form:
                    this.Walk(form);
                    break;
                case KnowledgeBase knowledgeBase:
                    this.Walk(knowledgeBase);
                    break;
                case ListExpression listExpression:
                    this.Walk(listExpression);
                    break;
                case LogicalPair logicalPair:
                    this.Walk(logicalPair);
                    break;
                case Term term:
                    this.Walk(term);
                    break;
                case VariableSpecification variableSpecification:
                    this.Walk(variableSpecification);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="Form"/>.
        /// </summary>
        /// <param name="form">The <see cref="Form"/> to walk.</param>
        public virtual void Walk(Form form)
        {
            switch (form)
            {
                case Definition definition:
                    this.Walk(definition);
                    break;
                case Sentence sentence:
                    this.Walk(sentence);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="FunctionalTerm"/>.
        /// </summary>
        /// <param name="functionalTerm">The <see cref="FunctionalTerm"/> to walk.</param>
        public virtual void Walk(FunctionalTerm functionalTerm)
        {
            switch (functionalTerm)
            {
                case ExplicitFunctionalTerm explicitFunctionalTerm:
                    this.Walk(explicitFunctionalTerm);
                    break;
                case ImplicitFunctionalTerm implicitFunctionalTerm:
                    this.Walk(implicitFunctionalTerm);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="IfTerm"/>.
        /// </summary>
        /// <param name="ifTerm">The <see cref="IfTerm"/> to walk.</param>
        public virtual void Walk(IfTerm ifTerm)
        {
            if (ifTerm.Pairs != null)
            {
                foreach (var logicalPair in ifTerm.Pairs)
                {
                    this.Walk((Expression)logicalPair);
                }
            }

            if (ifTerm.Default != null)
            {
                this.Walk((Expression)ifTerm.Default);
            }
        }

        /// <summary>
        /// Walks the <see cref="Implication"/>.
        /// </summary>
        /// <param name="implication">The <see cref="Implication"/> to walk.</param>
        public virtual void Walk(Implication implication)
        {
            if (implication.Antecedents != null)
            {
                foreach (var sentence in implication.Antecedents)
                {
                    this.Walk((Expression)sentence);
                }
            }

            if (implication.Consequent != null)
            {
                this.Walk((Expression)implication.Consequent);
            }
        }

        /// <summary>
        /// Walks the <see cref="ImplicitFunctionalTerm"/>.
        /// </summary>
        /// <param name="implicitFunctionalTerm">The <see cref="ImplicitFunctionalTerm"/> to walk.</param>
        public virtual void Walk(ImplicitFunctionalTerm implicitFunctionalTerm)
        {
            if (implicitFunctionalTerm.Function != null)
            {
                this.Walk((Expression)implicitFunctionalTerm.Function);
            }

            if (implicitFunctionalTerm.Arguments != null)
            {
                foreach (var term in implicitFunctionalTerm.Arguments)
                {
                    this.Walk((Expression)term);
                }
            }

            if (implicitFunctionalTerm.SequenceVariable != null)
            {
                this.Walk((Expression)implicitFunctionalTerm.SequenceVariable);
            }
        }

        /// <summary>
        /// Walks the <see cref="ImplicitRelationalSentence"/>.
        /// </summary>
        /// <param name="implicitRelationalSentence">The <see cref="ImplicitRelationalSentence"/> to walk.</param>
        public virtual void Walk(ImplicitRelationalSentence implicitRelationalSentence)
        {
            if (implicitRelationalSentence.Relation != null)
            {
                this.Walk((Expression)implicitRelationalSentence.Relation);
            }

            if (implicitRelationalSentence.Arguments != null)
            {
                foreach (var term in implicitRelationalSentence.Arguments)
                {
                    this.Walk((Expression)term);
                }
            }

            if (implicitRelationalSentence.SequenceVariable != null)
            {
                this.Walk((Expression)implicitRelationalSentence.SequenceVariable);
            }
        }

        /// <summary>
        /// Walks the <see cref="IndividualVariable"/>.
        /// </summary>
        /// <param name="individualVariable">The <see cref="IndividualVariable"/> to walk.</param>
        public virtual void Walk(IndividualVariable individualVariable)
        {
        }

        /// <summary>
        /// Walks the <see cref="Inequality"/>.
        /// </summary>
        /// <param name="inequality">The <see cref="Inequality"/> to walk.</param>
        public virtual void Walk(Inequality inequality)
        {
            if (inequality.Left != null)
            {
                this.Walk((Expression)inequality.Left);
            }

            if (inequality.Right != null)
            {
                this.Walk((Expression)inequality.Right);
            }
        }

        /// <summary>
        /// Walks the <see cref="KnowledgeBase"/>.
        /// </summary>
        /// <param name="knowledgeBase">The <see cref="KnowledgeBase"/> to walk.</param>
        public virtual void Walk(KnowledgeBase knowledgeBase)
        {
            if (knowledgeBase.Forms != null)
            {
                foreach (var form in knowledgeBase.Forms)
                {
                    this.Walk((Expression)form);
                }
            }
        }

        /// <summary>
        /// Walks the <see cref="ListExpression"/>.
        /// </summary>
        /// <param name="listExpression">The <see cref="ListExpression"/> to walk.</param>
        public virtual void Walk(ListExpression listExpression)
        {
            if (listExpression.Items != null)
            {
                foreach (var expression in listExpression.Items)
                {
                    this.Walk(expression);
                }
            }
        }

        /// <summary>
        /// Walks the <see cref="ListTerm"/>.
        /// </summary>
        /// <param name="listTerm">The <see cref="ListTerm"/> to walk.</param>
        public virtual void Walk(ListTerm listTerm)
        {
            if (listTerm.Items != null)
            {
                foreach (var term in listTerm.Items)
                {
                    this.Walk((Expression)term);
                }
            }

            if (listTerm.SequenceVariable != null)
            {
                this.Walk((Expression)listTerm.SequenceVariable);
            }
        }

        /// <summary>
        /// Walks the <see cref="LogicalPair"/>.
        /// </summary>
        /// <param name="logicalPair">The <see cref="LogicalPair"/> to walk.</param>
        public virtual void Walk(LogicalPair logicalPair)
        {
            if (logicalPair.Condition != null)
            {
                this.Walk((Expression)logicalPair.Condition);
            }

            if (logicalPair.Value != null)
            {
                this.Walk((Expression)logicalPair.Value);
            }
        }

        /// <summary>
        /// Walks the <see cref="LogicalSentence"/>.
        /// </summary>
        /// <param name="logicalSentence">The <see cref="LogicalSentence"/> to walk.</param>
        public virtual void Walk(LogicalSentence logicalSentence)
        {
            switch (logicalSentence)
            {
                case Conjunction conjunction:
                    this.Walk(conjunction);
                    break;
                case Disjunction disjunction:
                    this.Walk(disjunction);
                    break;
                case Equivalence equivalence:
                    this.Walk(equivalence);
                    break;
                case Implication implication:
                    this.Walk(implication);
                    break;
                case Negation negation:
                    this.Walk(negation);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="LogicalTerm"/>.
        /// </summary>
        /// <param name="logicalTerm">The <see cref="LogicalTerm"/> to walk.</param>
        public virtual void Walk(LogicalTerm logicalTerm)
        {
            switch (logicalTerm)
            {
                case ConditionalTerm conditionalTerm:
                    this.Walk(conditionalTerm);
                    break;
                case IfTerm ifTerm:
                    this.Walk(ifTerm);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="Negation"/>.
        /// </summary>
        /// <param name="negation">The <see cref="Negation"/> to walk.</param>
        public virtual void Walk(Negation negation)
        {
            if (negation.Negated != null)
            {
                this.Walk((Expression)negation.Negated);
            }
        }

        /// <summary>
        /// Walks the <see cref="Operator"/>.
        /// </summary>
        /// <param name="operator">The <see cref="Operator"/> to walk.</param>
        public virtual void Walk(Operator @operator)
        {
        }

        /// <summary>
        /// Walks the <see cref="PartialDefinition"/>.
        /// </summary>
        /// <param name="partialDefinition">The <see cref="PartialDefinition"/> to walk.</param>
        public virtual void Walk(PartialDefinition partialDefinition)
        {
            switch (partialDefinition)
            {
                case PartialFunctionDefinition partialFunctionDefinition:
                    this.Walk(partialFunctionDefinition);
                    break;
                case PartialLogicalDefinition partialLogicalDefinition:
                    this.Walk(partialLogicalDefinition);
                    break;
                case PartialObjectDefinition partialObjectDefinition:
                    this.Walk(partialObjectDefinition);
                    break;
                case PartialRelationDefinition partialRelationDefinition:
                    this.Walk(partialRelationDefinition);
                    break;
                case ReversePartialFunctionDefinition reversePartialFunctionDefinition:
                    this.Walk(reversePartialFunctionDefinition);
                    break;
                case ReversePartialLogicalDefinition reversePartialLogicalDefinition:
                    this.Walk(reversePartialLogicalDefinition);
                    break;
                case ReversePartialObjectDefinition reversePartialObjectDefinition:
                    this.Walk(reversePartialObjectDefinition);
                    break;
                case ReversePartialRelationDefinition reversePartialRelationDefinition:
                    this.Walk(reversePartialRelationDefinition);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="PartialFunctionDefinition"/>.
        /// </summary>
        /// <param name="partialFunctionDefinition">The <see cref="PartialFunctionDefinition"/> to walk.</param>
        public virtual void Walk(PartialFunctionDefinition partialFunctionDefinition)
        {
            if (partialFunctionDefinition.Constant != null)
            {
                this.Walk((Expression)partialFunctionDefinition.Constant);
            }

            if (partialFunctionDefinition.Description != null)
            {
                this.Walk((Expression)partialFunctionDefinition.Description);
            }

            if (partialFunctionDefinition.Parameters != null)
            {
                foreach (var individualVariable in partialFunctionDefinition.Parameters)
                {
                    this.Walk((Expression)individualVariable);
                }
            }

            if (partialFunctionDefinition.SequenceVariable != null)
            {
                this.Walk((Expression)partialFunctionDefinition.SequenceVariable);
            }

            if (partialFunctionDefinition.Variable != null)
            {
                this.Walk((Expression)partialFunctionDefinition.Variable);
            }

            if (partialFunctionDefinition.Sentence != null)
            {
                this.Walk((Expression)partialFunctionDefinition.Sentence);
            }
        }

        /// <summary>
        /// Walks the <see cref="PartialLogicalDefinition"/>.
        /// </summary>
        /// <param name="partialLogicalDefinition">The <see cref="PartialLogicalDefinition"/> to walk.</param>
        public virtual void Walk(PartialLogicalDefinition partialLogicalDefinition)
        {
            if (partialLogicalDefinition.Constant != null)
            {
                this.Walk((Expression)partialLogicalDefinition.Constant);
            }

            if (partialLogicalDefinition.Description != null)
            {
                this.Walk((Expression)partialLogicalDefinition.Description);
            }

            if (partialLogicalDefinition.Sentence != null)
            {
                this.Walk((Expression)partialLogicalDefinition.Sentence);
            }
        }

        /// <summary>
        /// Walks the <see cref="PartialObjectDefinition"/>.
        /// </summary>
        /// <param name="partialObjectDefinition">The <see cref="PartialObjectDefinition"/> to walk.</param>
        public virtual void Walk(PartialObjectDefinition partialObjectDefinition)
        {
            if (partialObjectDefinition.Constant != null)
            {
                this.Walk((Expression)partialObjectDefinition.Constant);
            }

            if (partialObjectDefinition.Description != null)
            {
                this.Walk((Expression)partialObjectDefinition.Description);
            }

            if (partialObjectDefinition.Variable != null)
            {
                this.Walk((Expression)partialObjectDefinition.Variable);
            }

            if (partialObjectDefinition.Sentence != null)
            {
                this.Walk((Expression)partialObjectDefinition.Sentence);
            }
        }

        /// <summary>
        /// Walks the <see cref="PartialRelationDefinition"/>.
        /// </summary>
        /// <param name="partialRelationDefinition">The <see cref="PartialRelationDefinition"/> to walk.</param>
        public virtual void Walk(PartialRelationDefinition partialRelationDefinition)
        {
            if (partialRelationDefinition.Constant != null)
            {
                this.Walk((Expression)partialRelationDefinition.Constant);
            }

            if (partialRelationDefinition.Description != null)
            {
                this.Walk((Expression)partialRelationDefinition.Description);
            }

            if (partialRelationDefinition.Parameters != null)
            {
                foreach (var individualVariable in partialRelationDefinition.Parameters)
                {
                    this.Walk((Expression)individualVariable);
                }
            }

            if (partialRelationDefinition.SequenceVariable != null)
            {
                this.Walk((Expression)partialRelationDefinition.SequenceVariable);
            }

            if (partialRelationDefinition.Sentence != null)
            {
                this.Walk((Expression)partialRelationDefinition.Sentence);
            }
        }

        /// <summary>
        /// Walks the <see cref="QuantifiedSentence"/>.
        /// </summary>
        /// <param name="quantifiedSentence">The <see cref="QuantifiedSentence"/> to walk.</param>
        public virtual void Walk(QuantifiedSentence quantifiedSentence)
        {
            switch (quantifiedSentence)
            {
                case ExistentiallyQuantifiedSentence existentiallyQuantifiedSentence:
                    this.Walk(existentiallyQuantifiedSentence);
                    break;
                case UniversallyQuantifiedSentence universallyQuantifiedSentence:
                    this.Walk(universallyQuantifiedSentence);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="Quotation"/>.
        /// </summary>
        /// <param name="quotation">The <see cref="Quotation"/> to walk.</param>
        public virtual void Walk(Quotation quotation)
        {
            if (quotation.Quoted != null)
            {
                this.Walk(quotation.Quoted);
            }
        }

        /// <summary>
        /// Walks the <see cref="RelationalSentence"/>.
        /// </summary>
        /// <param name="relationalSentence">The <see cref="RelationalSentence"/> to walk.</param>
        public virtual void Walk(RelationalSentence relationalSentence)
        {
            switch (relationalSentence)
            {
                case ExplicitRelationalSentence explicitRelationalSentence:
                    this.Walk(explicitRelationalSentence);
                    break;
                case ImplicitRelationalSentence implicitRelationalSentence:
                    this.Walk(implicitRelationalSentence);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="ReverseImplication"/>.
        /// </summary>
        /// <param name="reverseImplication">The <see cref="ReverseImplication"/> to walk.</param>
        public virtual void Walk(ReverseImplication reverseImplication)
        {
            if (reverseImplication.Consequent != null)
            {
                this.Walk((Expression)reverseImplication.Consequent);
            }

            if (reverseImplication.Antecedents != null)
            {
                foreach (var sentence in reverseImplication.Antecedents)
                {
                    this.Walk((Expression)sentence);
                }
            }
        }

        /// <summary>
        /// Walks the <see cref="ReversePartialFunctionDefinition"/>.
        /// </summary>
        /// <param name="reversePartialFunctionDefinition">The <see cref="ReversePartialFunctionDefinition"/> to walk.</param>
        public virtual void Walk(ReversePartialFunctionDefinition reversePartialFunctionDefinition)
        {
            if (reversePartialFunctionDefinition.Constant != null)
            {
                this.Walk((Expression)reversePartialFunctionDefinition.Constant);
            }

            if (reversePartialFunctionDefinition.Description != null)
            {
                this.Walk((Expression)reversePartialFunctionDefinition.Description);
            }

            if (reversePartialFunctionDefinition.Parameters != null)
            {
                foreach (var individualVariable in reversePartialFunctionDefinition.Parameters)
                {
                    this.Walk((Expression)individualVariable);
                }
            }

            if (reversePartialFunctionDefinition.SequenceVariable != null)
            {
                this.Walk((Expression)reversePartialFunctionDefinition.SequenceVariable);
            }

            if (reversePartialFunctionDefinition.Variable != null)
            {
                this.Walk((Expression)reversePartialFunctionDefinition.Variable);
            }

            if (reversePartialFunctionDefinition.Sentence != null)
            {
                this.Walk((Expression)reversePartialFunctionDefinition.Sentence);
            }
        }

        /// <summary>
        /// Walks the <see cref="ReversePartialLogicalDefinition"/>.
        /// </summary>
        /// <param name="reversePartialLogicalDefinition">The <see cref="ReversePartialLogicalDefinition"/> to walk.</param>
        public virtual void Walk(ReversePartialLogicalDefinition reversePartialLogicalDefinition)
        {
            if (reversePartialLogicalDefinition.Constant != null)
            {
                this.Walk((Expression)reversePartialLogicalDefinition.Constant);
            }

            if (reversePartialLogicalDefinition.Description != null)
            {
                this.Walk((Expression)reversePartialLogicalDefinition.Description);
            }

            if (reversePartialLogicalDefinition.Sentence != null)
            {
                this.Walk((Expression)reversePartialLogicalDefinition.Sentence);
            }
        }

        /// <summary>
        /// Walks the <see cref="ReversePartialObjectDefinition"/>.
        /// </summary>
        /// <param name="reversePartialObjectDefinition">The <see cref="ReversePartialObjectDefinition"/> to walk.</param>
        public virtual void Walk(ReversePartialObjectDefinition reversePartialObjectDefinition)
        {
            if (reversePartialObjectDefinition.Constant != null)
            {
                this.Walk((Expression)reversePartialObjectDefinition.Constant);
            }

            if (reversePartialObjectDefinition.Description != null)
            {
                this.Walk((Expression)reversePartialObjectDefinition.Description);
            }

            if (reversePartialObjectDefinition.Variable != null)
            {
                this.Walk((Expression)reversePartialObjectDefinition.Variable);
            }

            if (reversePartialObjectDefinition.Sentence != null)
            {
                this.Walk((Expression)reversePartialObjectDefinition.Sentence);
            }
        }

        /// <summary>
        /// Walks the <see cref="ReversePartialRelationDefinition"/>.
        /// </summary>
        /// <param name="reversePartialRelationDefinition">The <see cref="ReversePartialRelationDefinition"/> to walk.</param>
        public virtual void Walk(ReversePartialRelationDefinition reversePartialRelationDefinition)
        {
            if (reversePartialRelationDefinition.Constant != null)
            {
                this.Walk((Expression)reversePartialRelationDefinition.Constant);
            }

            if (reversePartialRelationDefinition.Description != null)
            {
                this.Walk((Expression)reversePartialRelationDefinition.Description);
            }

            if (reversePartialRelationDefinition.Parameters != null)
            {
                foreach (var individualVariable in reversePartialRelationDefinition.Parameters)
                {
                    this.Walk((Expression)individualVariable);
                }
            }

            if (reversePartialRelationDefinition.SequenceVariable != null)
            {
                this.Walk((Expression)reversePartialRelationDefinition.SequenceVariable);
            }

            if (reversePartialRelationDefinition.Sentence != null)
            {
                this.Walk((Expression)reversePartialRelationDefinition.Sentence);
            }
        }

        /// <summary>
        /// Walks the <see cref="Sentence"/>.
        /// </summary>
        /// <param name="sentence">The <see cref="Sentence"/> to walk.</param>
        public virtual void Walk(Sentence sentence)
        {
            switch (sentence)
            {
                case ConstantSentence constantSentence:
                    this.Walk(constantSentence);
                    break;
                case Equation equation:
                    this.Walk(equation);
                    break;
                case Inequality inequality:
                    this.Walk(inequality);
                    break;
                case LogicalSentence logicalSentence:
                    this.Walk(logicalSentence);
                    break;
                case QuantifiedSentence quantifiedSentence:
                    this.Walk(quantifiedSentence);
                    break;
                case RelationalSentence relationalSentence:
                    this.Walk(relationalSentence);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="SequenceVariable"/>.
        /// </summary>
        /// <param name="sequenceVariable">The <see cref="SequenceVariable"/> to walk.</param>
        public virtual void Walk(SequenceVariable sequenceVariable)
        {
        }

        /// <summary>
        /// Walks the <see cref="Term"/>.
        /// </summary>
        /// <param name="term">The <see cref="Term"/> to walk.</param>
        public virtual void Walk(Term term)
        {
            switch (term)
            {
                case CharacterReference characterReference:
                    this.Walk(characterReference);
                    break;
                case FunctionalTerm functionalTerm:
                    this.Walk(functionalTerm);
                    break;
                case ListTerm listTerm:
                    this.Walk(listTerm);
                    break;
                case LogicalTerm logicalTerm:
                    this.Walk(logicalTerm);
                    break;
                case Quotation quotation:
                    this.Walk(quotation);
                    break;
                case WordTerm wordTerm:
                    this.Walk(wordTerm);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="UniversallyQuantifiedSentence"/>.
        /// </summary>
        /// <param name="universallyQuantifiedSentence">The <see cref="UniversallyQuantifiedSentence"/> to walk.</param>
        public virtual void Walk(UniversallyQuantifiedSentence universallyQuantifiedSentence)
        {
            if (universallyQuantifiedSentence.Variables != null)
            {
                foreach (var variableSpecification in universallyQuantifiedSentence.Variables)
                {
                    this.Walk((Expression)variableSpecification);
                }
            }

            if (universallyQuantifiedSentence.Quantified != null)
            {
                this.Walk((Expression)universallyQuantifiedSentence.Quantified);
            }
        }

        /// <summary>
        /// Walks the <see cref="UnrestrictedDefinition"/>.
        /// </summary>
        /// <param name="unrestrictedDefinition">The <see cref="UnrestrictedDefinition"/> to walk.</param>
        public virtual void Walk(UnrestrictedDefinition unrestrictedDefinition)
        {
            switch (unrestrictedDefinition)
            {
                case UnrestrictedFunctionDefinition unrestrictedFunctionDefinition:
                    this.Walk(unrestrictedFunctionDefinition);
                    break;
                case UnrestrictedLogicalDefinition unrestrictedLogicalDefinition:
                    this.Walk(unrestrictedLogicalDefinition);
                    break;
                case UnrestrictedObjectDefinition unrestrictedObjectDefinition:
                    this.Walk(unrestrictedObjectDefinition);
                    break;
                case UnrestrictedRelationDefinition unrestrictedRelationDefinition:
                    this.Walk(unrestrictedRelationDefinition);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="UnrestrictedFunctionDefinition"/>.
        /// </summary>
        /// <param name="unrestrictedFunctionDefinition">The <see cref="UnrestrictedFunctionDefinition"/> to walk.</param>
        public virtual void Walk(UnrestrictedFunctionDefinition unrestrictedFunctionDefinition)
        {
            if (unrestrictedFunctionDefinition.Constant != null)
            {
                this.Walk((Expression)unrestrictedFunctionDefinition.Constant);
            }

            if (unrestrictedFunctionDefinition.Description != null)
            {
                this.Walk((Expression)unrestrictedFunctionDefinition.Description);
            }

            if (unrestrictedFunctionDefinition.Sentences != null)
            {
                foreach (var sentence in unrestrictedFunctionDefinition.Sentences)
                {
                    this.Walk((Expression)sentence);
                }
            }
        }

        /// <summary>
        /// Walks the <see cref="UnrestrictedLogicalDefinition"/>.
        /// </summary>
        /// <param name="unrestrictedLogicalDefinition">The <see cref="UnrestrictedLogicalDefinition"/> to walk.</param>
        public virtual void Walk(UnrestrictedLogicalDefinition unrestrictedLogicalDefinition)
        {
            if (unrestrictedLogicalDefinition.Constant != null)
            {
                this.Walk((Expression)unrestrictedLogicalDefinition.Constant);
            }

            if (unrestrictedLogicalDefinition.Description != null)
            {
                this.Walk((Expression)unrestrictedLogicalDefinition.Description);
            }

            if (unrestrictedLogicalDefinition.Sentences != null)
            {
                foreach (var sentence in unrestrictedLogicalDefinition.Sentences)
                {
                    this.Walk((Expression)sentence);
                }
            }
        }

        /// <summary>
        /// Walks the <see cref="UnrestrictedObjectDefinition"/>.
        /// </summary>
        /// <param name="unrestrictedObjectDefinition">The <see cref="UnrestrictedObjectDefinition"/> to walk.</param>
        public virtual void Walk(UnrestrictedObjectDefinition unrestrictedObjectDefinition)
        {
            if (unrestrictedObjectDefinition.Constant != null)
            {
                this.Walk((Expression)unrestrictedObjectDefinition.Constant);
            }

            if (unrestrictedObjectDefinition.Description != null)
            {
                this.Walk((Expression)unrestrictedObjectDefinition.Description);
            }

            if (unrestrictedObjectDefinition.Sentences != null)
            {
                foreach (var sentence in unrestrictedObjectDefinition.Sentences)
                {
                    this.Walk((Expression)sentence);
                }
            }
        }

        /// <summary>
        /// Walks the <see cref="UnrestrictedRelationDefinition"/>.
        /// </summary>
        /// <param name="unrestrictedRelationDefinition">The <see cref="UnrestrictedRelationDefinition"/> to walk.</param>
        public virtual void Walk(UnrestrictedRelationDefinition unrestrictedRelationDefinition)
        {
            if (unrestrictedRelationDefinition.Constant != null)
            {
                this.Walk((Expression)unrestrictedRelationDefinition.Constant);
            }

            if (unrestrictedRelationDefinition.Description != null)
            {
                this.Walk((Expression)unrestrictedRelationDefinition.Description);
            }

            if (unrestrictedRelationDefinition.Sentences != null)
            {
                foreach (var sentence in unrestrictedRelationDefinition.Sentences)
                {
                    this.Walk((Expression)sentence);
                }
            }
        }

        /// <summary>
        /// Walks the <see cref="Variable"/>.
        /// </summary>
        /// <param name="variable">The <see cref="Variable"/> to walk.</param>
        public virtual void Walk(Variable variable)
        {
            switch (variable)
            {
                case IndividualVariable individualVariable:
                    this.Walk(individualVariable);
                    break;
                case SequenceVariable sequenceVariable:
                    this.Walk(sequenceVariable);
                    break;
            }
        }

        /// <summary>
        /// Walks the <see cref="VariableSpecification"/>.
        /// </summary>
        /// <param name="variableSpecification">The <see cref="VariableSpecification"/> to walk.</param>
        public virtual void Walk(VariableSpecification variableSpecification)
        {
            if (variableSpecification.Variable != null)
            {
                this.Walk((Expression)variableSpecification.Variable);
            }

            if (variableSpecification.Constant != null)
            {
                this.Walk((Expression)variableSpecification.Constant);
            }
        }

        /// <summary>
        /// Walks the <see cref="WordTerm"/>.
        /// </summary>
        /// <param name="wordTerm">The <see cref="WordTerm"/> to walk.</param>
        public virtual void Walk(WordTerm wordTerm)
        {
            switch (wordTerm)
            {
                case Constant constant:
                    this.Walk(constant);
                    break;
                case Operator @operator:
                    this.Walk(@operator);
                    break;
                case Variable variable:
                    this.Walk(variable);
                    break;
            }
        }
    }
}
