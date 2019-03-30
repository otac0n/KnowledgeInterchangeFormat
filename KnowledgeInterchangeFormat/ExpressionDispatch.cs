// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat
{
    using System;
    using KnowledgeInterchangeFormat.Expressions;

    /// <summary>
    /// Provides expression dispatch.
    /// </summary>
    public static class ExpressionDispatch
    {
        /*
         * Generated with the following script:
         *
            var assembly = Assembly.LoadFrom(path);
            var expressionType = assembly.GetType("KnowledgeInterchangeFormat.Expressions.Expression");
            string typeName(Type t) => t.Name;
            var types = assembly.GetTypes().OrderBy(typeName).Where(t => expressionType.IsAssignableFrom(t)).ToList();
            string e(string name) => name == "operator" ? "@" + name : name;
            string lowerFirst(string name) => e(char.ToLowerInvariant(name[0]) + name.Substring(1));

            for (var i = 0; i < types.Count; i++)
            {
                var type = types[i].Name;
                $@"/// <param name=""{lowerFirst(type).TrimStart('@')}"">The action to be performed in the case of a <see cref=""{type}""/>.</param>".Dump();
            }
            $"Action<{expressionType.Name}> CreateDispatcher(".Dump();
            for (var i = 0; i < types.Count; i++)
            {
                var type = types[i].Name;
                $"    Action<{type}> {lowerFirst(type)} = null{(i == types.Count - 1 ? ")" : ",")}".Dump();
            }
            "{".Dump();

            var coalesced = new HashSet<Type>() { expressionType };
            void coalesce(Type type)
            {
                if (coalesced.Add(type))
                {
                    var @base = type.BaseType;
                    coalesce(@base);
                    $"    {lowerFirst(type.Name)} = {lowerFirst(type.Name)} ?? {lowerFirst(@base.Name)};".Dump();
                }
            }

            foreach (var type in types)
            {
                coalesce(type);
            }

            "    return expr =>".Dump();
            "    {".Dump();
            "".Dump();
            "        switch (expr)".Dump();
            "        {".Dump();

            var baseTypes = types.Where(b => types.Any(t => b != t && b.IsAssignableFrom(t))).ToSet();
            for (var i = 0; i < types.Count; i++)
            {
                if (baseTypes.Contains(types[i])) continue;
                var type = types[i].Name;
                $"            case {type} {lowerFirst(type)}Value:".Dump();
                $"                {lowerFirst(type)}?.Invoke({lowerFirst(type)}Value);".Dump();
                "                break;".Dump();
            }

            "        }".Dump();
            "    };".Dump();
            "}".Dump();
         */

        /// <summary>
        /// Creates a dispatcher function that will invoke the appropriate argument action when called.
        /// </summary>
        /// <param name="characterBlock">The action to be performed in the case of a <see cref="CharacterBlock"/>.</param>
        /// <param name="characterReference">The action to be performed in the case of a <see cref="CharacterReference"/>.</param>
        /// <param name="characterString">The action to be performed in the case of a <see cref="CharacterString"/>.</param>
        /// <param name="completeDefinition">The action to be performed in the case of a <see cref="CompleteDefinition"/>.</param>
        /// <param name="completeFunctionDefinition">The action to be performed in the case of a <see cref="CompleteFunctionDefinition"/>.</param>
        /// <param name="completeLogicalDefinition">The action to be performed in the case of a <see cref="CompleteLogicalDefinition"/>.</param>
        /// <param name="completeObjectDefinition">The action to be performed in the case of a <see cref="CompleteObjectDefinition"/>.</param>
        /// <param name="completeRelationDefinition">The action to be performed in the case of a <see cref="CompleteRelationDefinition"/>.</param>
        /// <param name="conditionalTerm">The action to be performed in the case of a <see cref="ConditionalTerm"/>.</param>
        /// <param name="conjunction">The action to be performed in the case of a <see cref="Conjunction"/>.</param>
        /// <param name="constant">The action to be performed in the case of a <see cref="Constant"/>.</param>
        /// <param name="constantSentence">The action to be performed in the case of a <see cref="ConstantSentence"/>.</param>
        /// <param name="definition">The action to be performed in the case of a <see cref="Definition"/>.</param>
        /// <param name="disjunction">The action to be performed in the case of a <see cref="Disjunction"/>.</param>
        /// <param name="equation">The action to be performed in the case of a <see cref="Equation"/>.</param>
        /// <param name="equivalence">The action to be performed in the case of a <see cref="Equivalence"/>.</param>
        /// <param name="existentiallyQuantifiedSentence">The action to be performed in the case of a <see cref="ExistentiallyQuantifiedSentence"/>.</param>
        /// <param name="explicitFunctionalTerm">The action to be performed in the case of a <see cref="ExplicitFunctionalTerm"/>.</param>
        /// <param name="explicitRelationalSentence">The action to be performed in the case of a <see cref="ExplicitRelationalSentence"/>.</param>
        /// <param name="expression">The action to be performed in the case of a <see cref="Expression"/>.</param>
        /// <param name="form">The action to be performed in the case of a <see cref="Form"/>.</param>
        /// <param name="functionalTerm">The action to be performed in the case of a <see cref="FunctionalTerm"/>.</param>
        /// <param name="ifTerm">The action to be performed in the case of a <see cref="IfTerm"/>.</param>
        /// <param name="implication">The action to be performed in the case of a <see cref="Implication"/>.</param>
        /// <param name="implicitFunctionalTerm">The action to be performed in the case of a <see cref="ImplicitFunctionalTerm"/>.</param>
        /// <param name="implicitRelationalSentence">The action to be performed in the case of a <see cref="ImplicitRelationalSentence"/>.</param>
        /// <param name="individualVariable">The action to be performed in the case of a <see cref="IndividualVariable"/>.</param>
        /// <param name="inequality">The action to be performed in the case of a <see cref="Inequality"/>.</param>
        /// <param name="knowledgeBase">The action to be performed in the case of a <see cref="KnowledgeBase"/>.</param>
        /// <param name="listExpression">The action to be performed in the case of a <see cref="ListExpression"/>.</param>
        /// <param name="listTerm">The action to be performed in the case of a <see cref="ListTerm"/>.</param>
        /// <param name="logicalPair">The action to be performed in the case of a <see cref="LogicalPair"/>.</param>
        /// <param name="logicalSentence">The action to be performed in the case of a <see cref="LogicalSentence"/>.</param>
        /// <param name="logicalTerm">The action to be performed in the case of a <see cref="LogicalTerm"/>.</param>
        /// <param name="negation">The action to be performed in the case of a <see cref="Negation"/>.</param>
        /// <param name="operator">The action to be performed in the case of a <see cref="Operator"/>.</param>
        /// <param name="partialDefinition">The action to be performed in the case of a <see cref="PartialDefinition"/>.</param>
        /// <param name="partialFunctionDefinition">The action to be performed in the case of a <see cref="PartialFunctionDefinition"/>.</param>
        /// <param name="partialLogicalDefinition">The action to be performed in the case of a <see cref="PartialLogicalDefinition"/>.</param>
        /// <param name="partialObjectDefinition">The action to be performed in the case of a <see cref="PartialObjectDefinition"/>.</param>
        /// <param name="partialRelationDefinition">The action to be performed in the case of a <see cref="PartialRelationDefinition"/>.</param>
        /// <param name="quantifiedSentence">The action to be performed in the case of a <see cref="QuantifiedSentence"/>.</param>
        /// <param name="quotation">The action to be performed in the case of a <see cref="Quotation"/>.</param>
        /// <param name="relationalSentence">The action to be performed in the case of a <see cref="RelationalSentence"/>.</param>
        /// <param name="reverseImplication">The action to be performed in the case of a <see cref="ReverseImplication"/>.</param>
        /// <param name="reversePartialFunctionDefinition">The action to be performed in the case of a <see cref="ReversePartialFunctionDefinition"/>.</param>
        /// <param name="reversePartialLogicalDefinition">The action to be performed in the case of a <see cref="ReversePartialLogicalDefinition"/>.</param>
        /// <param name="reversePartialObjectDefinition">The action to be performed in the case of a <see cref="ReversePartialObjectDefinition"/>.</param>
        /// <param name="reversePartialRelationDefinition">The action to be performed in the case of a <see cref="ReversePartialRelationDefinition"/>.</param>
        /// <param name="sentence">The action to be performed in the case of a <see cref="Sentence"/>.</param>
        /// <param name="sequenceVariable">The action to be performed in the case of a <see cref="SequenceVariable"/>.</param>
        /// <param name="term">The action to be performed in the case of a <see cref="Term"/>.</param>
        /// <param name="universallyQuantifiedSentence">The action to be performed in the case of a <see cref="UniversallyQuantifiedSentence"/>.</param>
        /// <param name="unrestrictedDefinition">The action to be performed in the case of a <see cref="UnrestrictedDefinition"/>.</param>
        /// <param name="unrestrictedFunctionDefinition">The action to be performed in the case of a <see cref="UnrestrictedFunctionDefinition"/>.</param>
        /// <param name="unrestrictedLogicalDefinition">The action to be performed in the case of a <see cref="UnrestrictedLogicalDefinition"/>.</param>
        /// <param name="unrestrictedObjectDefinition">The action to be performed in the case of a <see cref="UnrestrictedObjectDefinition"/>.</param>
        /// <param name="unrestrictedRelationDefinition">The action to be performed in the case of a <see cref="UnrestrictedRelationDefinition"/>.</param>
        /// <param name="variable">The action to be performed in the case of a <see cref="Variable"/>.</param>
        /// <param name="variableSpecification">The action to be performed in the case of a <see cref="VariableSpecification"/>.</param>
        /// <param name="wordTerm">The action to be performed in the case of a <see cref="WordTerm"/>.</param>
        /// <returns>A function that will dispatch a call to the appropriate argument action when invoked.</returns>
        public static Action<Expression> CreateDispatcher(
            Action<CharacterBlock> characterBlock = null,
            Action<CharacterReference> characterReference = null,
            Action<CharacterString> characterString = null,
            Action<CompleteDefinition> completeDefinition = null,
            Action<CompleteFunctionDefinition> completeFunctionDefinition = null,
            Action<CompleteLogicalDefinition> completeLogicalDefinition = null,
            Action<CompleteObjectDefinition> completeObjectDefinition = null,
            Action<CompleteRelationDefinition> completeRelationDefinition = null,
            Action<ConditionalTerm> conditionalTerm = null,
            Action<Conjunction> conjunction = null,
            Action<Constant> constant = null,
            Action<ConstantSentence> constantSentence = null,
            Action<Definition> definition = null,
            Action<Disjunction> disjunction = null,
            Action<Equation> equation = null,
            Action<Equivalence> equivalence = null,
            Action<ExistentiallyQuantifiedSentence> existentiallyQuantifiedSentence = null,
            Action<ExplicitFunctionalTerm> explicitFunctionalTerm = null,
            Action<ExplicitRelationalSentence> explicitRelationalSentence = null,
            Action<Expression> expression = null,
            Action<Form> form = null,
            Action<FunctionalTerm> functionalTerm = null,
            Action<IfTerm> ifTerm = null,
            Action<Implication> implication = null,
            Action<ImplicitFunctionalTerm> implicitFunctionalTerm = null,
            Action<ImplicitRelationalSentence> implicitRelationalSentence = null,
            Action<IndividualVariable> individualVariable = null,
            Action<Inequality> inequality = null,
            Action<KnowledgeBase> knowledgeBase = null,
            Action<ListExpression> listExpression = null,
            Action<ListTerm> listTerm = null,
            Action<LogicalPair> logicalPair = null,
            Action<LogicalSentence> logicalSentence = null,
            Action<LogicalTerm> logicalTerm = null,
            Action<Negation> negation = null,
            Action<Operator> @operator = null,
            Action<PartialDefinition> partialDefinition = null,
            Action<PartialFunctionDefinition> partialFunctionDefinition = null,
            Action<PartialLogicalDefinition> partialLogicalDefinition = null,
            Action<PartialObjectDefinition> partialObjectDefinition = null,
            Action<PartialRelationDefinition> partialRelationDefinition = null,
            Action<QuantifiedSentence> quantifiedSentence = null,
            Action<Quotation> quotation = null,
            Action<RelationalSentence> relationalSentence = null,
            Action<ReverseImplication> reverseImplication = null,
            Action<ReversePartialFunctionDefinition> reversePartialFunctionDefinition = null,
            Action<ReversePartialLogicalDefinition> reversePartialLogicalDefinition = null,
            Action<ReversePartialObjectDefinition> reversePartialObjectDefinition = null,
            Action<ReversePartialRelationDefinition> reversePartialRelationDefinition = null,
            Action<Sentence> sentence = null,
            Action<SequenceVariable> sequenceVariable = null,
            Action<Term> term = null,
            Action<UniversallyQuantifiedSentence> universallyQuantifiedSentence = null,
            Action<UnrestrictedDefinition> unrestrictedDefinition = null,
            Action<UnrestrictedFunctionDefinition> unrestrictedFunctionDefinition = null,
            Action<UnrestrictedLogicalDefinition> unrestrictedLogicalDefinition = null,
            Action<UnrestrictedObjectDefinition> unrestrictedObjectDefinition = null,
            Action<UnrestrictedRelationDefinition> unrestrictedRelationDefinition = null,
            Action<Variable> variable = null,
            Action<VariableSpecification> variableSpecification = null,
            Action<WordTerm> wordTerm = null)
        {
            term = term ?? expression;
            listTerm = listTerm ?? term;
            characterBlock = characterBlock ?? listTerm;
            characterReference = characterReference ?? term;
            characterString = characterString ?? listTerm;
            form = form ?? expression;
            definition = definition ?? form;
            completeDefinition = completeDefinition ?? definition;
            completeFunctionDefinition = completeFunctionDefinition ?? completeDefinition;
            completeLogicalDefinition = completeLogicalDefinition ?? completeDefinition;
            completeObjectDefinition = completeObjectDefinition ?? completeDefinition;
            completeRelationDefinition = completeRelationDefinition ?? completeDefinition;
            logicalTerm = logicalTerm ?? term;
            conditionalTerm = conditionalTerm ?? logicalTerm;
            sentence = sentence ?? form;
            logicalSentence = logicalSentence ?? sentence;
            conjunction = conjunction ?? logicalSentence;
            wordTerm = wordTerm ?? term;
            constant = constant ?? wordTerm;
            constantSentence = constantSentence ?? sentence;
            disjunction = disjunction ?? logicalSentence;
            equation = equation ?? sentence;
            equivalence = equivalence ?? logicalSentence;
            quantifiedSentence = quantifiedSentence ?? sentence;
            existentiallyQuantifiedSentence = existentiallyQuantifiedSentence ?? quantifiedSentence;
            functionalTerm = functionalTerm ?? term;
            explicitFunctionalTerm = explicitFunctionalTerm ?? functionalTerm;
            relationalSentence = relationalSentence ?? sentence;
            explicitRelationalSentence = explicitRelationalSentence ?? relationalSentence;
            ifTerm = ifTerm ?? logicalTerm;
            implication = implication ?? logicalSentence;
            implicitFunctionalTerm = implicitFunctionalTerm ?? functionalTerm;
            implicitRelationalSentence = implicitRelationalSentence ?? relationalSentence;
            variable = variable ?? wordTerm;
            individualVariable = individualVariable ?? variable;
            inequality = inequality ?? sentence;
            knowledgeBase = knowledgeBase ?? expression;
            listExpression = listExpression ?? expression;
            logicalPair = logicalPair ?? expression;
            negation = negation ?? logicalSentence;
            @operator = @operator ?? wordTerm;
            partialDefinition = partialDefinition ?? definition;
            partialFunctionDefinition = partialFunctionDefinition ?? partialDefinition;
            partialLogicalDefinition = partialLogicalDefinition ?? partialDefinition;
            partialObjectDefinition = partialObjectDefinition ?? partialDefinition;
            partialRelationDefinition = partialRelationDefinition ?? partialDefinition;
            quotation = quotation ?? term;
            reverseImplication = reverseImplication ?? implication;
            reversePartialFunctionDefinition = reversePartialFunctionDefinition ?? partialDefinition;
            reversePartialLogicalDefinition = reversePartialLogicalDefinition ?? partialDefinition;
            reversePartialObjectDefinition = reversePartialObjectDefinition ?? partialDefinition;
            reversePartialRelationDefinition = reversePartialRelationDefinition ?? partialDefinition;
            sequenceVariable = sequenceVariable ?? variable;
            universallyQuantifiedSentence = universallyQuantifiedSentence ?? quantifiedSentence;
            unrestrictedDefinition = unrestrictedDefinition ?? definition;
            unrestrictedFunctionDefinition = unrestrictedFunctionDefinition ?? unrestrictedDefinition;
            unrestrictedLogicalDefinition = unrestrictedLogicalDefinition ?? unrestrictedDefinition;
            unrestrictedObjectDefinition = unrestrictedObjectDefinition ?? unrestrictedDefinition;
            unrestrictedRelationDefinition = unrestrictedRelationDefinition ?? unrestrictedDefinition;
            variableSpecification = variableSpecification ?? expression;
            return expr =>
            {
                switch (expr)
                {
                    case CharacterBlock characterBlockValue:
                        characterBlock?.Invoke(characterBlockValue);
                        break;
                    case CharacterReference characterReferenceValue:
                        characterReference?.Invoke(characterReferenceValue);
                        break;
                    case CharacterString characterStringValue:
                        characterString?.Invoke(characterStringValue);
                        break;
                    case CompleteFunctionDefinition completeFunctionDefinitionValue:
                        completeFunctionDefinition?.Invoke(completeFunctionDefinitionValue);
                        break;
                    case CompleteLogicalDefinition completeLogicalDefinitionValue:
                        completeLogicalDefinition?.Invoke(completeLogicalDefinitionValue);
                        break;
                    case CompleteObjectDefinition completeObjectDefinitionValue:
                        completeObjectDefinition?.Invoke(completeObjectDefinitionValue);
                        break;
                    case CompleteRelationDefinition completeRelationDefinitionValue:
                        completeRelationDefinition?.Invoke(completeRelationDefinitionValue);
                        break;
                    case ConditionalTerm conditionalTermValue:
                        conditionalTerm?.Invoke(conditionalTermValue);
                        break;
                    case Conjunction conjunctionValue:
                        conjunction?.Invoke(conjunctionValue);
                        break;
                    case Constant constantValue:
                        constant?.Invoke(constantValue);
                        break;
                    case ConstantSentence constantSentenceValue:
                        constantSentence?.Invoke(constantSentenceValue);
                        break;
                    case Disjunction disjunctionValue:
                        disjunction?.Invoke(disjunctionValue);
                        break;
                    case Equation equationValue:
                        equation?.Invoke(equationValue);
                        break;
                    case Equivalence equivalenceValue:
                        equivalence?.Invoke(equivalenceValue);
                        break;
                    case ExistentiallyQuantifiedSentence existentiallyQuantifiedSentenceValue:
                        existentiallyQuantifiedSentence?.Invoke(existentiallyQuantifiedSentenceValue);
                        break;
                    case ExplicitFunctionalTerm explicitFunctionalTermValue:
                        explicitFunctionalTerm?.Invoke(explicitFunctionalTermValue);
                        break;
                    case ExplicitRelationalSentence explicitRelationalSentenceValue:
                        explicitRelationalSentence?.Invoke(explicitRelationalSentenceValue);
                        break;
                    case IfTerm ifTermValue:
                        ifTerm?.Invoke(ifTermValue);
                        break;
                    case ImplicitFunctionalTerm implicitFunctionalTermValue:
                        implicitFunctionalTerm?.Invoke(implicitFunctionalTermValue);
                        break;
                    case ImplicitRelationalSentence implicitRelationalSentenceValue:
                        implicitRelationalSentence?.Invoke(implicitRelationalSentenceValue);
                        break;
                    case IndividualVariable individualVariableValue:
                        individualVariable?.Invoke(individualVariableValue);
                        break;
                    case Inequality inequalityValue:
                        inequality?.Invoke(inequalityValue);
                        break;
                    case KnowledgeBase knowledgeBaseValue:
                        knowledgeBase?.Invoke(knowledgeBaseValue);
                        break;
                    case ListExpression listExpressionValue:
                        listExpression?.Invoke(listExpressionValue);
                        break;
                    case LogicalPair logicalPairValue:
                        logicalPair?.Invoke(logicalPairValue);
                        break;
                    case Negation negationValue:
                        negation?.Invoke(negationValue);
                        break;
                    case Operator @operatorValue:
                        @operator?.Invoke(@operatorValue);
                        break;
                    case PartialFunctionDefinition partialFunctionDefinitionValue:
                        partialFunctionDefinition?.Invoke(partialFunctionDefinitionValue);
                        break;
                    case PartialLogicalDefinition partialLogicalDefinitionValue:
                        partialLogicalDefinition?.Invoke(partialLogicalDefinitionValue);
                        break;
                    case PartialObjectDefinition partialObjectDefinitionValue:
                        partialObjectDefinition?.Invoke(partialObjectDefinitionValue);
                        break;
                    case PartialRelationDefinition partialRelationDefinitionValue:
                        partialRelationDefinition?.Invoke(partialRelationDefinitionValue);
                        break;
                    case Quotation quotationValue:
                        quotation?.Invoke(quotationValue);
                        break;
                    case ReverseImplication reverseImplicationValue:
                        reverseImplication?.Invoke(reverseImplicationValue);
                        break;
                    case ReversePartialFunctionDefinition reversePartialFunctionDefinitionValue:
                        reversePartialFunctionDefinition?.Invoke(reversePartialFunctionDefinitionValue);
                        break;
                    case ReversePartialLogicalDefinition reversePartialLogicalDefinitionValue:
                        reversePartialLogicalDefinition?.Invoke(reversePartialLogicalDefinitionValue);
                        break;
                    case ReversePartialObjectDefinition reversePartialObjectDefinitionValue:
                        reversePartialObjectDefinition?.Invoke(reversePartialObjectDefinitionValue);
                        break;
                    case ReversePartialRelationDefinition reversePartialRelationDefinitionValue:
                        reversePartialRelationDefinition?.Invoke(reversePartialRelationDefinitionValue);
                        break;
                    case SequenceVariable sequenceVariableValue:
                        sequenceVariable?.Invoke(sequenceVariableValue);
                        break;
                    case UniversallyQuantifiedSentence universallyQuantifiedSentenceValue:
                        universallyQuantifiedSentence?.Invoke(universallyQuantifiedSentenceValue);
                        break;
                    case UnrestrictedFunctionDefinition unrestrictedFunctionDefinitionValue:
                        unrestrictedFunctionDefinition?.Invoke(unrestrictedFunctionDefinitionValue);
                        break;
                    case UnrestrictedLogicalDefinition unrestrictedLogicalDefinitionValue:
                        unrestrictedLogicalDefinition?.Invoke(unrestrictedLogicalDefinitionValue);
                        break;
                    case UnrestrictedObjectDefinition unrestrictedObjectDefinitionValue:
                        unrestrictedObjectDefinition?.Invoke(unrestrictedObjectDefinitionValue);
                        break;
                    case UnrestrictedRelationDefinition unrestrictedRelationDefinitionValue:
                        unrestrictedRelationDefinition?.Invoke(unrestrictedRelationDefinitionValue);
                        break;
                    case VariableSpecification variableSpecificationValue:
                        variableSpecification?.Invoke(variableSpecificationValue);
                        break;
                }
            };
        }
    }
}
