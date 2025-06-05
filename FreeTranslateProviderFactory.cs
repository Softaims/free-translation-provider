using System;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using System.Windows.Forms;

[TranslationProviderFactory(Id = "OpenAITranslationProviderFactory",
                           Name = "OpenAI Translation Provider Factory",
                           Description = "OpenAI Translation Provider Factory")]
public class OpenAITranslationProviderFactory : ITranslationProviderFactory
{
    public static readonly string ListTranslationProviderScheme = "openaitranslationprovider";

    public ITranslationProvider CreateTranslationProvider(Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
    {
        if (!SupportsTranslationProviderUri(translationProviderUri))
        {
            throw new Exception("Cannot handle URI.");
        }

        var provider = new OpenAITranslationProvider(translationProviderUri, translationProviderState, credentialStore);
        return provider;
    }

    public bool SupportsTranslationProviderUri(Uri translationProviderUri)
    {
        if (translationProviderUri == null)
        {
            throw new ArgumentNullException("translationProviderUri");
        }
        return String.Equals(translationProviderUri.Scheme, ListTranslationProviderScheme, StringComparison.OrdinalIgnoreCase);
    }

    public TranslationProviderInfo GetTranslationProviderInfo(Uri translationProviderUri, string translationProviderState)
    {
        var info = new TranslationProviderInfo();
        info.TranslationMethod = TranslationMethod.MachineTranslation;
        info.Name = "Free Translator";
        return info;
    }

    public bool SupportsEditing
    {
        get { return true; }
    }

    public bool SupportsMultipleSelection
    {
        get { return false; }
    }

    public ITranslationProviderWinFormsUI GetTranslationProviderWinFormsUI(Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
    {
        return new OpenAITranslationProviderWinFormsUI();
    }
}