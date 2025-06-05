using System;
using System.Windows.Forms;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemoryApi;

public class OpenAITranslationProviderWinFormsUI : ITranslationProviderWinFormsUI
{
    public ITranslationProvider[] Browse(IWin32Window owner, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
    {
        var dialog = new OpenAITranslationProviderConfDialog();
        if (dialog.ShowDialog(owner) == DialogResult.OK)
        {
            var uri = new Uri("openaitranslationprovider:///");

            // Store the API key in credential store
            var credential = new TranslationProviderCredential(dialog.ApiKey, true);
            credentialStore.AddCredential(uri, credential);

            var provider = new OpenAITranslationProvider(uri, "", credentialStore);
            return new ITranslationProvider[] { provider };
        }
        return new ITranslationProvider[0];
    }

    public bool Edit(IWin32Window owner, ITranslationProvider translationProvider, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
    {
        var dialog = new OpenAITranslationProviderConfDialog();

        // Load existing API key if available
        var existingCredential = credentialStore.GetCredential(translationProvider.Uri);
        if (existingCredential != null)
        {
            dialog.ApiKey = existingCredential.Credential;
        }

        if (dialog.ShowDialog(owner) == DialogResult.OK)
        {
            var credential = new TranslationProviderCredential(dialog.ApiKey, true);
            credentialStore.AddCredential(translationProvider.Uri, credential);
            return true;
        }
        return false;
    }

    public bool GetCredentialsFromUser(IWin32Window owner, Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
    {
        var dialog = new OpenAITranslationProviderConfDialog();
        if (dialog.ShowDialog(owner) == DialogResult.OK)
        {
            var credential = new TranslationProviderCredential(dialog.ApiKey, true);
            credentialStore.AddCredential(translationProviderUri, credential);
            return true;
        }
        return false;
    }

    public TranslationProviderDisplayInfo GetDisplayInfo(Uri translationProviderUri, string translationProviderState)
    {
        var info = new TranslationProviderDisplayInfo();
        info.Name = "OpenAI Translation Provider";
        info.TooltipText = "OpenAI Translation Provider";
        info.SearchResultImage = null;
        info.TranslationProviderIcon = null;
        return info;
    }

    public bool SupportsEditing => true;

    public bool SupportsTranslationProviderUri(Uri translationProviderUri)
    {
        return translationProviderUri.Scheme == OpenAITranslationProviderFactory.ListTranslationProviderScheme;
    }

    public string TypeDescription => "OpenAI Translation Provider";

    public string TypeName => "OpenAI Translation Provider";
}