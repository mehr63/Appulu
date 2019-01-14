using Moq;
using Appulu.Core;
using Appulu.Core.Caching;
using Appulu.Core.Data;
using Appulu.Core.Domain.Localization;
using Appulu.Core.Domain.Seo;
using Appulu.Services.Localization;
using Appulu.Services.Seo;
using Appulu.Tests;
using NUnit.Framework;

namespace Appulu.Services.Tests.Seo
{
    [TestFixture]
    public class SeoExtensionsTests
    {
        private Mock<ILanguageService> _languageService;
        private Mock<IRepository<UrlRecord>> _urlRecordRepository;
        private Mock<IStaticCacheManager> _cacheManager;
        private Mock<IWorkContext> _workContext;
        private LocalizationSettings _localizationSettings;
        private SeoSettings _seoSettings;
        private IUrlRecordService _urlRecordService;

        [SetUp]
        public void SetUp()
        {
            _languageService=new Mock<ILanguageService>();
            _urlRecordRepository=new Mock<IRepository<UrlRecord>>();
            _cacheManager=new Mock<IStaticCacheManager>();
            _workContext=new Mock<IWorkContext>();
            _localizationSettings=new LocalizationSettings();
            _seoSettings=new SeoSettings();

            _urlRecordService = new UrlRecordService(_languageService.Object, _urlRecordRepository.Object,
                _cacheManager.Object, _workContext.Object, _localizationSettings, _seoSettings);
        }

        [Test]
        public void Should_return_lowercase()
        {
            _urlRecordService.GetSeName("tEsT", false, false).ShouldEqual("test");
        }

        [Test]
        public void Should_allow_all_latin_chars()
        {
            _urlRecordService.GetSeName("abcdefghijklmnopqrstuvwxyz1234567890", false, false).ShouldEqual("abcdefghijklmnopqrstuvwxyz1234567890");
        }

        [Test]
        public void Should_remove_illegal_chars()
        {
            _urlRecordService.GetSeName("test!@#$%^&*()+<>?/", false, false).ShouldEqual("test");
        }

        [Test]
        public void Should_replace_space_with_dash()
        {
            _urlRecordService.GetSeName("test test", false, false).ShouldEqual("test-test");
            _urlRecordService.GetSeName("test     test", false, false).ShouldEqual("test-test");
        }

        [Test]
        public void Can_convert_non_western_chars()
        {
            //German letters with diacritics
            _urlRecordService.GetSeName("testäöü", true, false).ShouldEqual("testaou");
            _urlRecordService.GetSeName("testäöü", false, false).ShouldEqual("test");
        }

        [Test]
        public void Can_allow_unicode_chars()
        {
            //Russian letters
            _urlRecordService.GetSeName("testтест", false, true).ShouldEqual("testтест");
            _urlRecordService.GetSeName("testтест", false, false).ShouldEqual("test");
        }
    }
}



