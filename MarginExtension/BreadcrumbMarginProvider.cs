﻿//------------------------------------------------------------------------------
// <copyright file="EditorMargin1Factory.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace MarginExtension
{
    /// <summary>Export a <see cref="IWpfTextViewMarginProvider"/></summary>
    [Export(typeof(IWpfTextViewMarginProvider))]
    [Name(BreadcrumbBarMargin.MarginName)]
    [Order(After = PredefinedMarginNames.Left)]
    [MarginContainer(PredefinedMarginNames.Top)]          
    [ContentType("F#")]            
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    [TextViewRole(PredefinedTextViewRoles.PrimaryDocument)]
    internal sealed class BreadcrumbMarginProvider : IWpfTextViewMarginProvider
    {
        /// <summary>
        /// Creates an <see cref="IWpfTextViewMargin"/> for the given <see cref="IWpfTextViewHost"/>.
        /// </summary>
        /// <param name="wpfTextViewHost">The <see cref="IWpfTextViewHost"/> for which to create the <see cref="IWpfTextViewMargin"/>.</param>
        /// <param name="marginContainer">The margin that will contain the newly-created margin.</param>
        /// <returns>The <see cref="IWpfTextViewMargin"/>.
        /// The value may be null if this <see cref="IWpfTextViewMarginProvider"/> does not participate for this context.
        /// </returns>
        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost wpfTextViewHost, IWpfTextViewMargin marginContainer)
        {
            return new BreadcrumbBarMargin(wpfTextViewHost.TextView, marginContainer);
        }
    }
}
