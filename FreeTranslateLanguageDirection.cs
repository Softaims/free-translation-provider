using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sdl.Core.Globalization;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

public class OpenAITranslationProviderLanguageDirection : ITranslationProviderLanguageDirection
{
    private readonly OpenAITranslationProvider _provider;
    private readonly LanguagePair _languageDirection;
    private static readonly HttpClient httpClient = new HttpClient();

    public OpenAITranslationProviderLanguageDirection(OpenAITranslationProvider provider, LanguagePair languageDirection)
    {
        _provider = provider;
        _languageDirection = languageDirection;
    }

    public System.Globalization.CultureInfo SourceLanguage => _languageDirection.SourceCulture;

    public System.Globalization.CultureInfo TargetLanguage => _languageDirection.TargetCulture;

    public ITranslationProvider TranslationProvider => _provider;

    public bool CanReverseLanguageDirection => true;

    CultureCode ITranslationProviderLanguageDirection.SourceLanguage => SourceLanguage;

    CultureCode ITranslationProviderLanguageDirection.TargetLanguage => TargetLanguage;

    public SearchResults SearchSegment(SearchSettings settings, Segment segment)
    {
        var translation = TranslateSegment(segment);
        var results = new SearchResults();
        results.SourceSegment = segment.Duplicate();

        if (!string.IsNullOrEmpty(translation))
        {
            var translatedSegment = new Segment(_languageDirection.TargetCulture);
            translatedSegment.Add(translation);

            // Create a TranslationUnit instead of passing Segment directly
            var translationUnit = new TranslationUnit()
            {
                SourceSegment = segment.Duplicate(),
                TargetSegment = translatedSegment
            };

            var searchResult = new SearchResult(translationUnit)
            {
                ScoringResult = new ScoringResult()
                {
                    BaseScore = 75
                }
            };

            results.Add(searchResult);
        }

        return results;
    }

    private string TranslateSegment(Segment segment)
    {
        try
        {
            var sourceText = segment.ToPlain();
            var apiKey = _provider.GetApiKey();

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("OpenAI API key not configured");
            }

            var task = TranslateTextAsync(sourceText, apiKey);
            task.Wait();
            return task.Result;
        }
        catch (Exception ex)
        {
            throw new Exception($"Translation failed: {ex.Message}");
        }
    }

    private async Task<string> TranslateTextAsync(string text, string apiKey)
    {
        var request = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = $"You are a professional translator. Translate the following text from {GetLanguageName(SourceLanguage)} to {GetLanguageName(TargetLanguage)}. Only provide the translation, no explanations."
                },
                new
                {
                    role = "user",
                    content = text
                }
            },
            max_tokens = 1000,
            temperature = 0.3
        };

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"OpenAI API error: {responseContent}");
        }

        dynamic result = JsonConvert.DeserializeObject(responseContent);
        return result.choices[0].message.content.ToString().Trim();
    }

    private string GetLanguageName(CultureInfo culture)
    {
        return culture.EnglishName;
    }

    public SearchResults[] SearchSegments(SearchSettings settings, Segment[] segments)
    {
        var results = new SearchResults[segments.Length];
        for (int i = 0; i < segments.Length; i++)
        {
            results[i] = SearchSegment(settings, segments[i]);
        }
        return results;
    }

    public SearchResults[] SearchSegmentsMasked(SearchSettings settings, Segment[] segments, bool[] mask)
    {
        if (segments == null)
        {
            throw new ArgumentNullException("segments");
        }
        if (mask == null || mask.Length != segments.Length)
        {
            throw new ArgumentException("mask");
        }

        var results = new SearchResults[segments.Length];
        for (int i = 0; i < segments.Length; ++i)
        {
            if (mask[i])
            {
                results[i] = SearchSegment(settings, segments[i]);
            }
            else
            {
                results[i] = new SearchResults();
                results[i].SourceSegment = segments[i].Duplicate();
            }
        }
        return results;
    }

    public SearchResults SearchText(SearchSettings settings, string segment)
    {
        var seg = new Segment(_languageDirection.SourceCulture);
        seg.Add(segment);
        return SearchSegment(settings, seg);
    }

    public SearchResults SearchTranslationUnit(SearchSettings settings, TranslationUnit translationUnit)
    {
        return SearchSegment(settings, translationUnit.SourceSegment);
    }

    public SearchResults[] SearchTranslationUnits(SearchSettings settings, TranslationUnit[] translationUnits)
    {
        var results = new SearchResults[translationUnits.Length];
        for (int i = 0; i < translationUnits.Length; i++)
        {
            results[i] = SearchTranslationUnit(settings, translationUnits[i]);
        }
        return results;
    }

    public SearchResults[] SearchTranslationUnitsMasked(SearchSettings settings, TranslationUnit[] translationUnits, bool[] mask)
    {
        var results = new SearchResults[translationUnits.Length];
        for (int i = 0; i < translationUnits.Length; i++)
        {
            if (mask == null || mask[i])
            {
                results[i] = SearchTranslationUnit(settings, translationUnits[i]);
            }
            else
            {
                results[i] = new SearchResults();
            }
        }
        return results;
    }

    #region "Not required"
    public ImportResult[] AddOrUpdateTranslationUnits(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings)
    {
        throw new NotImplementedException();
    }

    public ImportResult[] AddOrUpdateTranslationUnitsMasked(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings, bool[] mask)
    {
        throw new NotImplementedException();
    }

    public ImportResult AddTranslationUnit(TranslationUnit translationUnit, ImportSettings settings)
    {
        throw new NotImplementedException();
    }

    public ImportResult[] AddTranslationUnits(TranslationUnit[] translationUnits, ImportSettings settings)
    {
        throw new NotImplementedException();
    }

    public ImportResult[] AddTranslationUnitsMasked(TranslationUnit[] translationUnits, ImportSettings settings, bool[] mask)
    {
        throw new NotImplementedException();
    }

    public bool DeleteAllTranslationUnits()
    {
        throw new NotImplementedException();
    }

    public int DeleteTranslationUnits(TranslationUnit[] translationUnits)
    {
        throw new NotImplementedException();
    }

    public int DeleteTranslationUnitsWithIterator(ref RegularIterator iterator)
    {
        throw new NotImplementedException();
    }


    public RegularIterator GetRegularIterator()
    {
        throw new NotImplementedException();
    }

    public int GetTranslationUnitCount()
    {
        throw new NotImplementedException();
    }

    public ImportResult UpdateTranslationUnit(TranslationUnit translationUnit)
    {
        throw new NotImplementedException();
    }

    public ImportResult[] UpdateTranslationUnits(TranslationUnit[] translationUnits)
    {
        throw new NotImplementedException();
    }
    #endregion
}