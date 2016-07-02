using System;
using System.Collections.Generic;
using System.Linq;
using LinkedinFetcher.Common.Interfaces;
using LinkedinFetcher.Common.Models;
using LinkedinFetcher.DataProvider.Cache;
using LinkedinFetcher.DataProvider.LinkedIn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LinkedinFetcher.Tests.DataProviders
{
    [TestClass]
    public class LinkedInProfileProviderTest
    {
        #region Init
        private IProfileProvider _profileProvider;

        [TestInitialize]
        public void TestInit()
        {
            //Arrange.
            var mockDowloader = new Mock<IHtmlDownloader>();
            mockDowloader.Setup(s => s.DownloadHtml(It.IsIn(
                new[] { "normal", "https://il.linkedin.com/in/talbronfer" })))
                .Returns(GetRegularHtml);
            mockDowloader.Setup(s => s.DownloadHtml(It.Is<string>(s1 => s1 == "empty"))).Returns(String.Empty);
            mockDowloader.Setup(s => s.DownloadHtml(It.Is<string>(s1 => s1 == "invalid"))).Returns("who let the dogs out");

            /* this mock cache provider will allways return fresh data :*/
            var mockCacheProvider = new Mock<ICacheProvider<Profile>>();
            mockCacheProvider.Setup(s => s.GetCachedData(It.IsAny<Func<string, Profile>>(), It.IsAny<string>()))
                .Returns((Func<string, Profile> freshDataFunc, string parameter) => freshDataFunc(parameter));

            _profileProvider = new LinkedinProfileProvider(new LinkedinHtmlParser(), mockDowloader.Object, mockCacheProvider.Object);
        }

        private static string GetRegularHtml()
        {
            const string html =
                @"
                <!DOCTYPE html>
                <!--[if lt IE 7]> <html lang='en' class='ie ie6 lte9 lte8 lte7 os-win'> <![endif]-->
                <!--[if IE 7]> <html lang='en' class='ie ie7 lte9 lte8 lte7 os-win'> <![endif]-->
                <!--[if IE 8]> <html lang='en' class='ie ie8 lte9 lte8 os-win'> <![endif]-->
                <!--[if IE 9]> <html lang='en' class='ie ie9 lte9 os-win'> <![endif]-->
                <!--[if gt IE 9]> <html lang='en' class='os-win'> <![endif]-->
                <!--[if !IE]><!--> <html lang='en' class='os-win'> <!--<![endif]-->
                <head>
                <meta charset='UTF-8'/>
                <title>Tal Bronfer | LinkedIn</title>
                <meta name='referrer' content='origin'/>
                <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                <meta name='pageImpressionID' content='3e9d01f5-d8c4-4e1f-99c5-7b28f08521b0'>
                <meta name='appName' content='chrome'>
                <meta name='pageKey' content='public_profile_v3_desktop'>
                <meta name='treeID' content='aZzZHPBNXBSgvOBCkSsAAA=='>
                <meta name='globalTrackingUrl' content='//www.linkedin.com/mob/tracking'>
                <meta name='globalTrackingAppName' content='chrome'>
                <meta name='globalTrackingAppId' content='webTracking'>
                <!--[if lte IE 8]>
                  <link rel='shortcut icon' href='https://static.licdn.com/scds/common/u/images/logos/favicons/v1/16x16/favicon.ico'>
                <![endif]-->
                <!--[if IE 9]>
                  <link rel='shortcut icon' href='https://static.licdn.com/scds/common/u/images/logos/favicons/v1/favicon.ico'>
                <![endif]-->
                <link rel='icon' href='https://static.licdn.com/scds/common/u/images/logos/favicons/v1/favicon.ico'>
                <link rel='apple-touch-icon-precomposed' href='https://static.licdn.com/scds/common/u/img/icon/apple-touch-icon.png'>
                <meta name='msapplication-TileImage' content='https://static.licdn.com/scds/common/u/images/logos/linkedin/logo-in-win8-tile-144_v1.png'/>
                <meta name='msapplication-TileColor' content='#0077B5'/>
                <meta name='application-name' content='LinkedIn'/>
                <meta content='https://static.licdn.com/scds/concat/common/js?v=build-2000_8_56815-prod' name='RemoteNavJSContentBaseURL'/>
                <script type='text/javascript' src='https://static.licdn.com/scds/concat/common/js?h=7ndrn0f9fw0hum7uoqcjcnzne-95d8d303rtd0n9wj4dcjbnh2c-7vr4nuab43rzvy2pgq7yvvxjk-9qa4rfxekcw3lt2c06h7p0kmf'></script>
                <link rel='stylesheet' type='text/css' href='https://static.licdn.com/scds/concat/common/css?h=6cr4z4epp0a5mxufg332nezs4-a4kjc5uqttio53azw54aex6s3-as8kt5bqspxc01tl9cizqa37j'>
                <script></script>
                <!--[if IE 8]><script src='https://static.licdn.com/scds/common/u/lib/polyfill/1.0.2/ie8-polyfill.min.js'></script><![endif]-->
                <!--[if IE 9]><script src='https://static.licdn.com/scds/common/u/lib/polyfill/1.0.2/ie9-polyfill.min.js'></script><![endif]-->
                <meta name='lnkd-track-json-lib' content='https://static.licdn.com/scds/concat/common/js?h=2jds9coeh4w78ed9wblscv68v-ebbt2vixcc5qz0otts5io08xv'>
                <meta name='lnkd-track-lib' content='https://static.licdn.com/scds/concat/common/js?h=ebbt2vixcc5qz0otts5io08xv'>
                <meta name='lnkd-track-error' content='/lite/ua/error?csrfToken=ajax%3A2451404493697687063'>
                <script type='text/javascript' src='https://static.licdn.com/scds/concat/common/js?h=e2lgukqldpqool72t8g7tysag-3nuvxgwg15rbghxm1gpzfbya2-1nm61x5u7981e88m10hpaekkm-mv3v66b8q0h1hvgvd3yfjv5f-14k913qahq3mh0ac0lh0twk9v'></script>
                <link rel='canonical' href='https://il.linkedin.com/in/talbronfer'/><link rel='stylesheet' href='https://static.licdn.com/sc/h/76zh7hi5a29lucdbc128kptsk'/><script type='text/javascript' src='https://static.licdn.com/sc/h/4tcd0mh70bs89zecpaumh27p1'></script><meta http-equiv='content-type' content='text/html; charset=UTF-8'><meta name='robots' content='noarchive'><meta name='description' content='View Tal Bronfer’s professional profile on LinkedIn. LinkedIn is the world&#39;s largest business network, helping professionals like Tal Bronfer discover inside connections to recommended job candidates, industry experts, and business partners.'/><meta name='og:title' content='Tal Bronfer | LinkedIn'/><meta name='og:description' content='View Tal Bronfer’s professional profile on LinkedIn. LinkedIn is the world&#39;s largest business network, helping professionals like Tal Bronfer discover inside connections to recommended job candidates, industry experts, and business partners.'/>
                </head>
                <body class='js guest' dir='ltr' id='pagekey-public_profile_v3_desktop'>
                <div id='application-body'>
                <header id='layout-header' class='layout-header-or-footer' role='banner'>
                <div id='header-banner'>
                <div class='wrapper'>
                <a class='banner-logo-container guest' href='http://www.linkedin.com/'>
                <h2 id='li-logo' class='logo'>
                LinkedIn Home
                </h2>
                </a>
                <nav role='navigation' class='nav guest-nav' aria-label='Main site navigation'>
                <ul role='menubar'>
                <li role='menuitem' class='nav-item'>
                <a class='nav-link' href='http://www.linkedin.com/static?key=what_is_linkedin&amp;trk=hb_what'>
                What is LinkedIn?
                </a>
                </li>
                <li role='menuitem' class='nav-item'>
                <a rel='nofollow' class='nav-link' href='/start/join?trk=hb_join'>
                Join Today
                </a>
                </li>
                <li role='menuitem' class='nav-item'>
                <a rel='nofollow' class='nav-link' href='https://www.linkedin.com/uas/login?goback=&amp;trk=hb_signin'>
                Sign In
                </a>
                </li>
                </ul>
                </nav>
                </div>
                </div>
                <div class='a11y-content'>
                <a id='a11y-content-link' tabindex='0' name='a11y-content'>Main content starts below.</a>
                </div>
                <script data-page-js-type='lix'>(function(n,r,a){r=window[n]=window[n]||{};r['jsecure_injectAlert']='control';r['jsecure_Dialog']='control';}('__li__lix_registry__'));</script>
                </header>
                <main id='layout-main' role='main'>
                <code id='__pageContext__' style='display: none;'><!--{'baseScdsUrl':'https://static.licdn.com/scds','contextPath':'/public-profile/','isProd':true,'linkedInDustJsUrl':'https://static.licdn.com/sc/h/4tcd0mh70bs89zecpaumh27p1','baseSparkUrlForHashes':'https://static.licdn.com/sc/h','isCsUser':false,'appName':'public-profile-frontend','fizzyJsUrl':'https://static.licdn.com/scds/common/u/lib/fizzy/fz-1.3.3-min.js','mpName':'public-profile-frontend','scHashesUrl':'https://static.licdn.com/sc/p/com.linkedin.public-profile-frontend%3Apublic-profile-frontend-static-content%2B0.0.485/f/%2Fpublic-profile-frontend%2Fsc-hashes%2Fsc-hashes_en_US.js','dustDebug':'control','baseMediaUrl':'https://media.licdn.com/media','useCdn':true,'locale':'en_US','version':'0.0.485','useScHashesJs':false,'cdnUrl':'https://static.licdn.com','baseMprUrl':'https://media.licdn.com/mpr/mpr','playUtilsUrl':'https://static.licdn.com/sc/h/744ngu9n49o9ruwbfn52vwgka','useNativeXmsg':false,'hashesDisabledByQueryParam':false,'baseAssetsUrl':'https://static.licdn.com/sc/p/com.linkedin.public-profile-frontend%3Apublic-profile-frontend-static-content%2B0.0.485/f','csrfToken':'ajax:2451404493697687063','intlPolyfillUrl':'https://static.licdn.com/sc/h/1fw1ey0jfgqapy4dndtgrr7y1','serveT8WithDust':false,'disableDynamicConcat':false,'baseSparkUrlForFiles':'https://static.licdn.com/sc/p/com.linkedin.public-profile-frontend%3Apublic-profile-frontend-static-content%2B0.0.485/f','dustUtilsUrl':'https://static.licdn.com/sc/h/eekxmw2mg4t0hh3rax0naa6bb','linkedInDustI18nJsUrl':'https://static.licdn.com/sc/h/d6j7otrxjqmrjpw0r4fz02dsg','baseMediaProxyUrl':'https://media.licdn.com/media-proxy'}--></code><script src='https://static.licdn.com/sc/h/eekxmw2mg4t0hh3rax0naa6bb'></script><code id='isMobile' style='display: none;'><!--false--></code><code id='pageKey' style='display: none;'><!--'public_profile_v3_desktop'--></code><code id='jsBeaconUrl' style='display: none;'><!--'https://il.linkedin.com/analytics/noauthtracker'--></code><code id='lixPublicProfileInJoinWallEnabled' style='display: none;'><!--'control'--></code><div id='wrapper'><div id='profile'><section id='topcard' class='profile-section'><div class='profile-card vcard no-picture'><div class='profile-overview'><div class='profile-overview-content'><h1 id='name' class='fn'>Tal Bronfer</h1><p class='headline title' data-section='headline'><span class='translation' data-text-toggle='' data-field-name='Headline'> Software Developer at Gartner Innovation Center </span></p><dl id='demographics'><dt>Location</dt><dd class='descriptor adr'><span class='locality'>Israel</span></dd><dt>Industry</dt><dd class='descriptor'>Computer Software</dd></dl><table class='extra-info'><tbody><tr data-section='currentPositionsDetails'><th>Current</th><td><ol><li><span class='org'><a href='https://www.linkedin.com/company/gartner?trk=ppro_cprof'>Gartner</a></span></li></ol></td></tr><tr data-section='pastPositionsDetails'><th>Previous</th><td><ol><li><a href='https://www.linkedin.com/company/safebreach?trk=ppro_cprof'>SafeBreach</a>, </li><li><a href='https://www.linkedin.com/company/fullr?trk=ppro_cprof'>Fullr</a>, </li><li><a href='https://www.linkedin.com/company/israel-defense-forces?trk=ppro_cprof'>Israel Defense Forces</a></li></ol></td></tr><tr><th>Recommendations</th><td><strong>1</strong> person has recommended <strong>Tal</strong></td></tr></tbody></table><div class='member-connections'><strong>384</strong> connections</div></div></div></div><div class='reg-upsell  advocate-cta-enabled lazy-load'><h2>View Tal’s full profile. It's&nbsp;free!</h2><h3>Your colleagues, classmates, and 400 million other professionals are on LinkedIn.</h3><a href='https://www.linkedin.com/start/view-full-profile?_ed=0_8zZXIevOFjyDZh-vEAFUrzGwH27vcQi6jXYmNKIPcjg81F1u8Za3bnF2DkXKxomW&amp;trk=pprof-0-ts-view_full-0' class='signup-button'>View Tal’s Full Profile</a></div></section><section id='education' data-section='educationsDetails' class='profile-section'><h3 class='title'>Education</h3><ul class='schools'><li class='school'><header><h4 class='item-title'><span class='translation' data-text-toggle='' data-field-name='Education.SchoolName'> the Open University of Israel </span></h4><h5 class='item-subtitle'><span class='translation' data-text-toggle='Bachelor of Science (BSc), Applied Science' data-field-name='Education.DegreeName'> Bachelor of Science (BSc), Applied Science </span></h5></header><div class='meta'><span class='date-range'><time>2012</time> – <time>2017</time></span></div></li><li class='school'><header><h4 class='item-title'><span class='translation' data-text-toggle='' data-field-name='Education.SchoolName'> The Israel Arts and Science Academy </span></h4><h5 class='item-subtitle'><span class='translation' data-text-toggle='High School, Natural Sciences' data-field-name='Education.DegreeName'> High School, Natural Sciences </span></h5></header><div class='meta'><span class='date-range'><time>2007</time> – <time>2010</time></span></div><div class='description' data-section='educations'><p>The Israel Arts and Science Academy (IASA) is a high school in Jerusalem for gifted students from all over Israel. Majored in computer science and biology.</p></div></li></ul></section><section id='summary' data-section='summary' class='profile-section'><h3 class='title'>Summary</h3><div class='description'><p>Full-stack developer specializing in Web Apps. Keen to learn, well-motivated and passionate about technology, entrepreneurship and innovation.</p></div></section><section id='experience' class='profile-section'><h3 class='title'>Experience</h3><ul class='positions'><li class='position' data-section='currentPositionsDetails'><header><h5 class='logo'><a href='https://www.linkedin.com/company/gartner?trk=ppro_cprof'><img data-delayed-url='https://media.licdn.com/media/p/2/000/01a/22d/3cea61b.png' class='lazy-load' alt='Gartner'></a></h5><h4 class='item-title'><a href='https://il.linkedin.com/title/software-developer?trk=pprofile_title'><span class='translation' data-text-toggle='' data-field-name='Position.Title'> Software Developer </span></a></h4><h5 class='item-subtitle'><a href='https://www.linkedin.com/company/gartner?trk=ppro_cprof'><span class='translation' data-text-toggle='' data-field-name='Position.CompanyName'> Gartner </span></a></h5></header><div class='meta'><span class='date-range'><time>May 2016</time> – Present (2 months)</span></div></li><li class='position' data-section='pastPositionsDetails'><header><h5 class='logo'><a href='https://www.linkedin.com/company/safebreach?trk=ppro_cprof'><img data-delayed-url='https://media.licdn.com/media/AAEAAQAAAAAAAAKOAAAAJGE2YTFiY2IzLTAxZjktNGE5Zi1iYmY1LTkxNGQ5NzM5NTEwMg.png' class='lazy-load' alt='SafeBreach'></a></h5><h4 class='item-title'><a href='https://il.linkedin.com/title/software-developer?trk=pprofile_title'><span class='translation' data-text-toggle='' data-field-name='Position.Title'> Software Developer </span></a></h4><h5 class='item-subtitle'><a href='https://www.linkedin.com/company/safebreach?trk=ppro_cprof'><span class='translation' data-text-toggle='' data-field-name='Position.CompanyName'> SafeBreach </span></a></h5></header><div class='meta'><span class='date-range'><time>May 2015</time> – <time>May 2016</time> (1 year 1 month)</span></div><p class='description' data-section='pastPositions'>- Designing & implementing Cloud-based REST APIs for the company's on-premise security solution using Node.js, MongoDB and the Express & Swagger frameworks.<br><br>- Working with the company's UX department on developing the product's customer-facing UI using Angular.js. This includes dealing with many challenges in the modern SPA world, including authentication, communication with multiple external APIs, cross-device responsiveness & client-side performance.<br><br>- Serving as the Point of Contact for the company's production Cloud environment, including responsibility for Continuous Integration & Delivery solutions, redundancy, applicative security and other aspects.<br><br>- Working with BDD tests written in Yadda (Karma) and with JS task frameworks such as Gulp.</p></li><li class='position' data-section='pastPositionsDetails'><header><h5 class='logo'><a href='https://www.linkedin.com/company/fullr?trk=ppro_cprof'><img data-delayed-url='https://media.licdn.com/media/p/6/005/0af/39e/38c9596.png' class='lazy-load' alt='Fullr'></a></h5><h4 class='item-title'><a href='https://il.linkedin.com/title/co-founder?trk=pprofile_title'><span class='translation' data-text-toggle='' data-field-name='Position.Title'> Co-founder </span></a></h4><h5 class='item-subtitle'><a href='https://www.linkedin.com/company/fullr?trk=ppro_cprof'><span class='translation' data-text-toggle='' data-field-name='Position.CompanyName'> Fullr </span></a></h5></header><div class='meta'><span class='date-range'><time>December 2014</time> – <time>April 2015</time> (5 months)</span></div><p class='description' data-section='pastPositions'>Fullr is creating a smart Connected Car device and a mobile application, making car ownership easier and cheaper by managing all car-related expenses in a personalized, effortless manner.</p></li><li class='position' data-section='pastPositionsDetails'><header><h4 class='item-title'><a href='https://il.linkedin.com/title/full-stack-software-developer?trk=pprofile_title'><span class='translation' data-text-toggle='' data-field-name='Position.Title'> Full-Stack Software Developer </span></a></h4><h5 class='item-subtitle'><a href='https://www.linkedin.com/company/israel-defense-forces?trk=ppro_cprof'><span class='translation' data-text-toggle='' data-field-name='Position.CompanyName'> Israel Defense Forces </span></a></h5></header><div class='meta'><span class='date-range'><time>January 2014</time> – <time>January 2015</time> (1 year 1 month)</span></div><p class='description' data-section='pastPositions'>- Designing and developing Single Page Applications in an R&D team specializing in rapid, Agile-based implementation of new concepts<br><br>- Full stack responsibility for undertaken projects, implementing server-side in .NET and client-side in AngularJS, jQuery and various other frameworks</p></li><li class='position' data-section='pastPositionsDetails'><header><h4 class='item-title'><a href='https://il.linkedin.com/title/software-automation-engineer?trk=pprofile_title'><span class='translation' data-text-toggle='' data-field-name='Position.Title'> Software Automation Engineer </span></a></h4><h5 class='item-subtitle'><a href='https://www.linkedin.com/company/israel-defense-forces?trk=ppro_cprof'><span class='translation' data-text-toggle='' data-field-name='Position.CompanyName'> Israel Defense Forces </span></a></h5></header><div class='meta'><span class='date-range'><time>February 2013</time> – <time>March 2014</time> (1 year 2 months)</span><span class='location'>Israel</span></div><p class='description' data-section='pastPositions'>- Designed & developed continuously-integrating software test automation infrastructures and solutions for enterprise-scale .NET and Web-based systems deployed to thousands of users<br><br>- Responsible for professional management and guidance of a team of several software test automation developers working with the above-mentioned infrastructures.<br><br>- Researching and implementing new automated testing tools & methodologies in the organization.</p></li><li class='position' data-section='pastPositionsDetails'><header><h4 class='item-title'><a href='https://il.linkedin.com/title/software-qa-engineer?trk=pprofile_title'><span class='translation' data-text-toggle='' data-field-name='Position.Title'> Software QA Engineer </span></a></h4><h5 class='item-subtitle'><a href='https://www.linkedin.com/company/israel-defense-forces?trk=ppro_cprof'><span class='translation' data-text-toggle='' data-field-name='Position.CompanyName'> Israel Defense Forces </span></a></h5></header><div class='meta'><span class='date-range'><time>April 2011</time> – <time>February 2013</time> (1 year 11 months)</span><span class='location'>Israel</span></div><p class='description' data-section='pastPositions'>- Designed testing methodologies in user-oriented corporate software systems. <br><br>- Designed & developed software tools for automation of various QA processes, including configuration and deployment management, production data analysis and testing in production. <br><br>- Responsible for detailing customer business processes into requirements and development tasks.<br><br>- Performed testing cycles while debugging, analyzing and prioritizing defects.</p></li><li class='position' data-section='pastPositionsDetails'><header><h5 class='logo'><a href='https://www.linkedin.com/company/the-truth-about-cars?trk=ppro_cprof'><img data-delayed-url='https://media.licdn.com/media/p/3/000/09a/087/040f995.png' class='lazy-load' alt='TheTruthAboutCars.com'></a></h5><h4 class='item-title'><a href='https://il.linkedin.com/title/contributing-writer?trk=pprofile_title'><span class='translation' data-text-toggle='' data-field-name='Position.Title'> Contributing Writer </span></a></h4><h5 class='item-subtitle'><a href='https://www.linkedin.com/company/the-truth-about-cars?trk=ppro_cprof'><span class='translation' data-text-toggle='' data-field-name='Position.CompanyName'> TheTruthAboutCars.com </span></a></h5></header><div class='meta'><span class='date-range'><time>December 2009</time> – <time>December 2012</time> (3 years 1 month)</span></div><p class='description' data-section='pastPositions'>- Responsible for covering & providing in-depth analysis of the Better Place project <br><br>- Conducted road tests and wrote reviews of various cars</p></li></ul></section><section id='languages' data-section='languages' class='profile-section'><h3 class='title'>Languages</h3><ul><li class='language'><div class='wrap'><h4 class='name'><span class='translation' data-text-toggle='' data-field-name='Language.Name'> Hebrew </span></h4><p class='proficiency'></p></div></li><li class='language'><div class='wrap'><h4 class='name'><span class='translation' data-text-toggle='' data-field-name='Language.Name'> English </span></h4><p class='proficiency'></p></div></li></ul></section><section id='skills' data-section='skills' class='profile-section skills-section'><h3 class='title'>Skills</h3><ul class='pills'><input type='checkbox' id='skills-expander-state-checkbox' class='expander-state-checkbox'/><li class='skill'><a href='https://il.linkedin.com/topic/angularjs?trk=pprofile_topic' title='AngularJS'><span class='wrap'>AngularJS</span></a></li><li class='skill'><a href='https://il.linkedin.com/topic/node%2Ejs?trk=pprofile_topic' title='Node.js'><span class='wrap'>Node.js</span></a></li><li class='skill'><a href='https://il.linkedin.com/topic/c%23?trk=pprofile_topic' title='C#'><span class='wrap'>C#</span></a></li><li class='skill'><a href='https://il.linkedin.com/topic/%2Enet?trk=pprofile_topic' title='.NET'><span class='wrap'>.NET</span></a></li><li class='skill'><a href='https://il.linkedin.com/topic/mongodb?trk=pprofile_topic' title='MongoDB'><span class='wrap'>MongoDB</span></a></li><li class='skill'><a href='https://il.linkedin.com/topic/jquery?trk=pprofile_topic' title='jQuery'><span class='wrap'>jQuery</span></a></li><li class='skill'><a href='https://il.linkedin.com/topic/asp%2Enet-mvc?trk=pprofile_topic' title='ASP.NET MVC'><span class='wrap'>ASP.NET MVC</span></a></li><li class='skill'><a href='https://il.linkedin.com/topic/asp-%2Enet-web-api?trk=pprofile_topic' title='ASP .NET Web API'><span class='wrap'>ASP .NET Web API</span></a></li><li class='skill'><a href='https://il.linkedin.com/topic/web-applications?trk=pprofile_topic' title='Web Applications'><span class='wrap'>Web Applications</span></a></li><li class='skill'><a href='https://il.linkedin.com/topic/javascript?trk=pprofile_topic' title='JavaScript'><span class='wrap'>JavaScript</span></a></li><li class='skill'><a href='https://il.linkedin.com/topic/sql?trk=pprofile_topic' title='SQL'><span class='wrap'>SQL</span></a></li><li class='skill'><a href='https://il.linkedin.com/topic/selenium?trk=pprofile_topic' title='Selenium'><span class='wrap'>Selenium</span></a></li><li class='skill'><a href='https://il.linkedin.com/topic/web-development?trk=pprofile_topic' title='Web Development'><span class='wrap'>Web Development</span></a></li></ul><div class='translation-voter-wrap'><div class='translation-voter'><span>How&#39;s this translation?</span><ul class='voter-form'><li class='action option positive'><span class='action positive'>Great</span></li><li class='separator'>&#8226;</li><li class='action option negative'><span class='action negative'>Has errors</span></li></ul></div><div class='translation-voter-response'>Thanks for your help!</div></div></section><section id='volunteering' data-section='volunteering' class='profile-section'><h3 class='title'>Volunteer Experience &amp; Causes</h3><ul><li class='position'><header><h4 class='item-title'><span class='translation' data-text-toggle='' data-field-name='VolunteeringExperience.Role'> Co-Organizer </span></h4><h5 class='item-subtitle'>Geekim Hackathon</h5></header><div class='meta no-image'><span class='date-range'><time>November 2014</time> – Present (1 year 8 months)</span><span class='cause'>Education</span></div><p class='description'>Co-organized the Geekim Hackathon, the first hackathon intended for active servants and alumni of the Intelligence Corps, where over a 100 participants helped develop apps and software solutions to technologically help and solve causes in the Israeli society, such as accessibility for the disabled, poverty, environment and others.</p></li><li class='position'><header><h4 class='item-title'><span class='translation' data-text-toggle='' data-field-name='VolunteeringExperience.Role'> Training Officer </span></h4><h5 class='item-subtitle'><a href='https://www.linkedin.com/company/israel-defense-forces?trk=ppro_cprof'>Israel Defense Forces</a></h5></header><div class='meta no-image'><span class='date-range'><time>March 2014</time> – Present (2 years 4 months)</span><span class='cause'>Education</span></div><p class='description'>Served twice as a training officer and later a point of contact for the Intelligence Corps' primary software development course, Mamas. Responsible for researching and developing lessons and exercises in the .NET, Web and Software Automation worlds, as well as developing the course's management system and other technological aides.</p></li><li class='position'><header><h4 class='item-title'><span class='translation' data-text-toggle='' data-field-name='VolunteeringExperience.Role'> Co-Organizer </span></h4><h5 class='item-subtitle'>Geekim Hackathon</h5></header><div class='meta no-image'><span class='date-range'><time>November 2015</time> – Present (8 months)</span><span class='cause'>Education</span></div><p class='description'>Co-organized the 2015 Geekim Hackathon held for the second consecutive year. Responsible for raising funds, recruiting sponsors from the Israeli startup community, creating the judging format for the event and managing public relations.</p></li><li class='position'><header><h4 class='item-title'><span class='translation' data-text-toggle='' data-field-name='VolunteeringExperience.Role'> Group Mentor </span></h4><h5 class='item-subtitle'>Magshimim</h5></header><div class='meta no-image'><span class='date-range'><time>November 2015</time> – Present (8 months)</span><span class='cause'>Education</span></div><p class='description'>The Magshimim initiative, led by the Israeli Ministry of Defense and the Rashi Foundation, is a practical program teaching excelling high school students cyber-security related subjects, including software development & networks. As a Group Mentor, I help a group of students with planning & executing their year-long final project, in both technical and business-related domains.</p></li></ul></section><section id='recommendations' data-section='recommendations' class='profile-section'><h3 class='title'>Recommendations</h3><div class='content'><p class='description'>A preview of what LinkedIn members have to say about Tal:</p><ul><li class='recommendation-container'><input type='checkbox' id='recommendation-state-0' class='recommendation-state'><blockquote class='recommendation'>Tal is a talented full-stack developer. I had the opportunity to work with Tal on a couple of projects where he had full responsibility over the client-side architecture (using Angular.js at all times) and each time he got it way over every expectations.
                Great to work with!</blockquote><label for='recommendation-state-0' class='see-more-action'>See more</label><label for='recommendation-state-0' class='see-less-action'>See less</label></li></ul><a class='signup-link' href='https://www.linkedin.com/start/view-full-profile?_ed=0_8zZXIevOFjyDZh-vEAFUrzGwH27vcQi6jXYmNKIPcjg81F1u8Za3bnF2DkXKxomW&amp;trk=pprof-0-ts-view_full-0'>Sign up to see who recommended Tal</a></div></section><section id='groups' data-section='groups' class='profile-section'><h3 class='title'>Groups</h3><ul><input type='checkbox' id='groups-expander-state-checkbox' class='expander-state-checkbox'/><li class='group'><h5 class='logo'><a href='https://il.linkedin.com/groups?gid=3457396&amp;trk=fulpro_grplogo'><img data-delayed-url='https://media.licdn.com/media/p/2/000/073/18c/2efac5b.png' alt='.NET Experts in Israel' class='lazy-load'></a></h5><h4 class='item-title'><a href='https://il.linkedin.com/groups?gid=3457396&amp;trk=prof-groups-membership-name'>.NET Experts in Israel</a></h4></li><li class='group'><h5 class='logo'><a href='https://il.linkedin.com/groups?gid=3308723&amp;trk=fulpro_grplogo'><img data-delayed-url='https://media.licdn.com/media/p/1/000/073/22c/167ab44.png' alt='Israel High Tech ++' class='lazy-load'></a></h5><h4 class='item-title'><a href='https://il.linkedin.com/groups?gid=3308723&amp;trk=prof-groups-membership-name'>Israel High Tech ++</a></h4></li><li class='group'><h5 class='logo'><a href='https://il.linkedin.com/groups?gid=85746&amp;trk=fulpro_grplogo'><img data-delayed-url='https://media.licdn.com/media/p/3/000/007/2f5/24e22e3.png' alt='C# Developers / Architects' class='lazy-load'></a></h5><h4 class='item-title'><a href='https://il.linkedin.com/groups?gid=85746&amp;trk=prof-groups-membership-name'>C# Developers / Architects</a></h4></li><li class='group'><h5 class='logo'><a href='https://il.linkedin.com/groups?gid=4574416&amp;trk=fulpro_grplogo'><img data-delayed-url='https://media.licdn.com/media/AAEAAQAAAAAAAAYyAAAAJGYyOTQzOTU2LTkxZDQtNDFlOC1iYTFlLWViYWVmZDI1ZGZhZA.png' alt='EcoMotion Israel' class='lazy-load'></a></h5><h4 class='item-title'><a href='https://il.linkedin.com/groups?gid=4574416&amp;trk=prof-groups-membership-name'>EcoMotion Israel</a></h4></li><li class='group'><h5 class='logo'><a href='https://il.linkedin.com/groups?gid=120650&amp;trk=fulpro_grplogo'><img data-delayed-url='https://media.licdn.com/media/p/2/000/012/1cf/2231de3.png' alt='Test Automation' class='lazy-load'></a></h5><h4 class='item-title'><a href='https://il.linkedin.com/groups?gid=120650&amp;trk=prof-groups-membership-name'>Test Automation</a></h4></li><li class='group'><h5 class='logo'><a href='https://il.linkedin.com/groups?gid=4971846&amp;trk=fulpro_grplogo'><img data-delayed-url='https://media.licdn.com/media/p/2/000/263/108/1be66e9.png' alt='Developers Network in Israel' class='lazy-load'></a></h5><h4 class='item-title'><a href='https://il.linkedin.com/groups?gid=4971846&amp;trk=prof-groups-membership-name'>Developers Network in Israel</a></h4></li></ul><div class='reg-upsell '><h2>View Tal’s full profile to...</h2><ul><li>See who you know in common</li><li>Get introduced</li><li>Contact <strong>Tal</strong> directly</li></ul><a href='https://www.linkedin.com/start/view-full-profile?_ed=0_8zZXIevOFjyDZh-vEAFUrzGwH27vcQi6jXYmNKIPcjg81F1u8Za3bnF2DkXKxomW&amp;trk=pprof-0-ts-view_full-0' class='signup-button'>View Tal’s Full Profile</a></div></section></div><div id='aux'><section class='insights profile-section'><div class='name-search'><h3 class='title'>Search by name</h3><p class='blurb'>Over 400 million professionals are already on LinkedIn. Find who you know.</p><form action='https://il.linkedin.com/pub/dir/' method='get'><label for='firstName'>First Name</label><input type='text' name='first' id='firstName' placeholder='First Name'><label for='lastName'>Last Name</label><input type='text' name='last' id='lastName' placeholder='Last Name'><input type='hidden' name='trk' value='prof-samename-search-submit'><button aria-label='Search' type='submit'><span></span></button><p class='tip'><strong>Example: </strong> <a href='https://il.linkedin.com/in/jeffweiner08'>Jeff Weiner</a></p></form></div><div class='browse-map'><h3 class='title'>People Also Viewed</h3><ul><li class='profile-card'><a href='https://il.linkedin.com/in/degoltz?trk=pub-pbmap'><img lazyLoad='true' class='lazy-load' data-delayed-url='https://media.licdn.com/mpr/mpr/shrink_100_100/p/2/005/0af/334/016d849.jpg' width='100' height='100'/></a><div class='info'><h4 class='item-title'><a href='https://il.linkedin.com/in/degoltz?trk=pub-pbmap'>Daniel Goltz</a></h4><p class='headline'>Full-Stack .NET Developer</p></div></li><li class='profile-card'><a href='https://il.linkedin.com/in/eric-feldman-30259a88?trk=pub-pbmap'><img lazyLoad='true' class='lazy-load' data-delayed-url='https://media.licdn.com/mpr/mpr/shrink_100_100/p/2/005/09c/130/11efd11.jpg' width='100' height='100'/></a><div class='info'><h4 class='item-title'><a href='https://il.linkedin.com/in/eric-feldman-30259a88?trk=pub-pbmap'>Eric Feldman</a></h4><p class='headline'>Software Developer at Israel Defense Forces</p></div></li><li class='profile-card'><a href='https://il.linkedin.com/in/shanielh?trk=pub-pbmap'><img lazyLoad='true' class='lazy-load' data-delayed-url='https://media.licdn.com/mpr/mpr/shrink_100_100/p/1/000/220/02b/0ff9f6e.jpg' width='100' height='100'/></a><div class='info'><h4 class='item-title'><a href='https://il.linkedin.com/in/shanielh?trk=pub-pbmap'>Shani Elharrar</a></h4><p class='headline'>Software Developer</p></div></li><li class='profile-card'><a href='https://il.linkedin.com/in/hagaiblochgadot?trk=pub-pbmap'><img lazyLoad='true' class='lazy-load' data-delayed-url='https://media.licdn.com/mpr/mpr/shrink_100_100/AAEAAQAAAAAAAAd5AAAAJDhjZjA1NmRhLTAxNjYtNDk0MC1iNzk5LTNhNzI4OWZjNTI1ZA.jpg' width='100' height='100'/></a><div class='info'><h4 class='item-title'><a href='https://il.linkedin.com/in/hagaiblochgadot?trk=pub-pbmap'>Hagai Bloch Gadot</a></h4><p class='headline'>Software Developer at IDF</p></div></li><li class='profile-card'><a href='https://il.linkedin.com/in/yahel-yechieli-38291981?trk=pub-pbmap'><img lazyLoad='true' class='lazy-load' data-delayed-url='https://media.licdn.com/mpr/mpr/shrink_100_100/AAEAAQAAAAAAAAYkAAAAJDg1N2IxYTUwLTRiOGEtNDc5My05ODUyLTA1NDIyMTAzOTAzOQ.jpg' width='100' height='100'/></a><div class='info'><h4 class='item-title'><a href='https://il.linkedin.com/in/yahel-yechieli-38291981?trk=pub-pbmap'>Yahel Yechieli</a></h4><p class='headline'>Front-End\U​X Team Leader at IDF</p></div></li><li class='profile-card'><a href='https://il.linkedin.com/in/chen-shusterman-a8582889?trk=pub-pbmap'><img lazyLoad='true' class='lazy-load' data-delayed-url='https://media.licdn.com/mpr/mpr/shrink_100_100/p/7/005/0a0/337/0cab436.jpg' width='100' height='100'/></a><div class='info'><h4 class='item-title'><a href='https://il.linkedin.com/in/chen-shusterman-a8582889?trk=pub-pbmap'>Chen Shusterman</a></h4><p class='headline'>Software Developer</p></div></li><li class='profile-card'><a href='https://il.linkedin.com/in/lidor-alon-6245b644?trk=pub-pbmap'><img lazyLoad='true' class='lazy-load' data-delayed-url='https://media.licdn.com/mpr/mpr/shrink_100_100/p/6/005/062/248/31cced1.jpg' width='100' height='100'/></a><div class='info'><h4 class='item-title'><a href='https://il.linkedin.com/in/lidor-alon-6245b644?trk=pub-pbmap'>lidor alon</a></h4><p class='headline'>Server API developer at Viber</p></div></li><li class='profile-card'><a href='https://il.linkedin.com/in/natali-nisim-18598480?trk=pub-pbmap'><img lazyLoad='true' class='lazy-load' data-delayed-url='https://media.licdn.com/mpr/mpr/shrink_100_100/AAEAAQAAAAAAAAZcAAAAJDM0ZDBhNmQ5LTE4YzktNDg1MC1iNDYwLTczODY2MGFjOTI0Zg.jpg' width='100' height='100'/></a><div class='info'><h4 class='item-title'><a href='https://il.linkedin.com/in/natali-nisim-18598480?trk=pub-pbmap'>Natali Nisim</a></h4><p class='headline'>Software developer</p></div></li><li class='profile-card'><a href='https://il.linkedin.com/in/ben-hodeda-26178578?trk=pub-pbmap'><img lazyLoad='true' class='lazy-load' data-delayed-url='https://media.licdn.com/mpr/mpr/shrink_100_100/AAEAAQAAAAAAAAf5AAAAJDNiNWNjOWMzLTJjMGItNDc5ZC04YmIyLWY1NjkzM2NlODQ1OA.jpg' width='100' height='100'/></a><div class='info'><h4 class='item-title'><a href='https://il.linkedin.com/in/ben-hodeda-26178578?trk=pub-pbmap'>Ben Hodeda</a></h4><p class='headline'>Israel Defense Forces</p></div></li><li class='profile-card'><a href='https://il.linkedin.com/in/roie-labes-913a19b7?trk=pub-pbmap'><img lazyLoad='true' class='lazy-load' data-delayed-url='https://media.licdn.com/mpr/mpr/shrink_100_100/AAEAAQAAAAAAAAI6AAAAJDA5ODcxMmVhLWZjMmItNGE1NC1hOWFiLTBjZjExNWRlOWNkMQ.jpg' width='100' height='100'/></a><div class='info'><h4 class='item-title'><a href='https://il.linkedin.com/in/roie-labes-913a19b7?trk=pub-pbmap'>Roie Labes</a></h4><p class='headline'>Software Programmer and Team Leader</p></div></li></ul></div></section><section class='badge profile-section'><h3 class='title'>Public profile badge</h3><p class='description'>Include this LinkedIn profile on other websites</p><a class='cta' href='https://www.linkedin.com/badges/profile/create?vanityname=talbronfer&amp;trk=profile-badge-public-profile'>View profile badges</a></section><section id='ad' class='profile-section'><iframe width='300' scrolling='no' height='250' frameborder='0' allowtransparency='true' border='0' data-src='/csp/dtag?sz=300x250&ti=2&p=1&z=full_profile&_x=%3Bcompany%3D786353942%3Bpcntry%3Dil'></iframe><script>window.addEventListener('load',function(){var iframes=document.getElementsByTagName('iframe');for(var i=0;i<iframes.length;i++){var iframe=iframes[i],src=iframe.getAttribute('data-src');if(src){iframe.src=src;}}});</script></section></div><div id='directory' class='il'><h3 class='title'>LinkedIn members in Israel:</h3><ol type='a' class='primary'><li><a href='https://il.linkedin.com/directory/people-a/'>a</a></li><li><a href='https://il.linkedin.com/directory/people-b/'>b</a></li><li><a href='https://il.linkedin.com/directory/people-c/'>c</a></li><li><a href='https://il.linkedin.com/directory/people-d/'>d</a></li><li><a href='https://il.linkedin.com/directory/people-e/'>e</a></li><li><a href='https://il.linkedin.com/directory/people-f/'>f</a></li><li><a href='https://il.linkedin.com/directory/people-g/'>g</a></li><li><a href='https://il.linkedin.com/directory/people-h/'>h</a></li><li><a href='https://il.linkedin.com/directory/people-i/'>i</a></li><li><a href='https://il.linkedin.com/directory/people-j/'>j</a></li><li><a href='https://il.linkedin.com/directory/people-k/'>k</a></li><li><a href='https://il.linkedin.com/directory/people-l/'>l</a></li><li><a href='https://il.linkedin.com/directory/people-m/'>m</a></li><li><a href='https://il.linkedin.com/directory/people-n/'>n</a></li><li><a href='https://il.linkedin.com/directory/people-o/'>o</a></li><li><a href='https://il.linkedin.com/directory/people-p/'>p</a></li><li><a href='https://il.linkedin.com/directory/people-q/'>q</a></li><li><a href='https://il.linkedin.com/directory/people-r/'>r</a></li><li><a href='https://il.linkedin.com/directory/people-s/'>s</a></li><li><a href='https://il.linkedin.com/directory/people-t/'>t</a></li><li><a href='https://il.linkedin.com/directory/people-u/'>u</a></li><li><a href='https://il.linkedin.com/directory/people-v/'>v</a></li><li><a href='https://il.linkedin.com/directory/people-w/'>w</a></li><li><a href='https://il.linkedin.com/directory/people-x/'>x</a></li><li><a href='https://il.linkedin.com/directory/people-y/'>y</a></li><li><a href='https://il.linkedin.com/directory/people-z/'>z</a></li><li><a href='https://il.linkedin.com/directory/people-1/'>more</a></li><li class='country-search'>Browse members <a href='https://www.linkedin.com/directory/country_listing/'>by country</a></li></ol></div><script type='text/javascript'>(function(){var DOMAIN_NAME=window.location.hostname.replace(/.*\.([^\.]+\.[^\.]+)/,'$1'),DOMAIN_NAME_PROD='linkedin.com';function getRemoteFrameUrl(){return('https:'===document.location.protocol?'https://':'http://')+'platform.'+DOMAIN_NAME+'/js/thirdPartyJSDelegatorFrame.html';}function loadExternalTracking(){var thirdPartyJs=new Espany({remote:getRemoteFrameUrl(),ready:function(){if(DOMAIN_NAME!==DOMAIN_NAME_PROD){return;}thirdPartyJs.loadScript('analyticsjs').result(function(msg){thirdPartyJs.run('ga',['create','UA-62256447-1',{'cookieDomain':DOMAIN_NAME_PROD}]);thirdPartyJs.run('ga',['set','referrer',document.referrer]);thirdPartyJs.run('ga',['set','location',window.location.href]);thirdPartyJs.run('ga',['send','pageview',{page:'public_profile_v3_desktop'}]);});}});}window.addEventListener('load',loadExternalTracking);})();</script></div>
                </main>
                <script type='text/javascript' src='https://static.licdn.com/scds/concat/common/js?h=a06jpss2hf43xwxobn0gl598m-44hhbxag3hinac547ym9vby09-a4lcy9x33w9gvnro4s0fw3e8z-9zz2lhu3eq1epk7sq1t8cdb5s-d40s93k758bei48k0v4jpzeyi-9o2gces8tdiaq46j2fgjkg6d4-bctwwqj7p01tcj2smshz2bboe-aaykw1861wb5yl2yeseicumeh-bftaa82sjwcbrohoe28skni7b-2r8hqscu9unerft0cqwr58gz1-acapv3trxf5gmj7o87qomcp3f-cfabcg4u1cj0em4yissh5mfxu'></script>
                <footer id='layout-footer' class='layout-header-or-footer' role='contentinfo'>
                <div class='wrapper'>
                <p class='copyright guest'><span>LinkedIn Corporation</span> <em>&copy; 2016</em></p>
                <ul class='nav-legal' role='navigation' aria-label='Footer Legal Menu'>
                <li role='menuitem'><a href='http://www.linkedin.com/legal/user-agreement?trk=hb_ft_userag'>User Agreement</a></li>
                <li role='menuitem'><a href='http://www.linkedin.com/legal/privacy-policy?trk=hb_ft_priv'>Privacy Policy</a></li>
                <li role='menuitem'><a href='https://linkedin.com/help/linkedin/answer/34593?lang=en'>Community Guidelines</a></li>
                <li role='menuitem'><a href='http://www.linkedin.com/legal/cookie-policy?trk=hb_ft_cookie'>Cookie Policy</a></li>
                <li role='menuitem'><a href='http://www.linkedin.com/legal/copyright-policy?trk=hb_ft_copy'>Copyright Policy</a></li>
                <li role='menuitem'><a href='/psettings/guest-email-unsubscribe?trk=hb_ft_gunsub' rel='nofollow'>Unsubscribe</a></li>
                </ul>
                </div>
                </footer>
                <script data-page-js-type='config'>(function(n,r,a){r=window[n]=window[n]||{};a=r['WebTracking']=r['WebTracking']||{};a['URLs']={'saveWebActionTrackURL':'\/lite\/secure-web-action-track?csrfToken=ajax%3A2451404493697687063'};}('__li__config_registry__'));</script>
                <script data-page-js-type='lix'>(function(n,r,a){r=window[n]=window[n]||{};r['global_bsp_notice_type']='warning';r['global_bsp_notice_autoHide']='false';r['global_bsp_view_threshold']='c5';}('__li__lix_registry__'));</script>
                <script data-page-js-type='i18n'>(function(n,r,a){r=window[n]=window[n]||{};r['global_browser_unsupported_notice']='Looks like you\'re using a browser that\'s not supported. <a target=\'_blank\' href=\'https:\/\/linkedin.com\/help\/linkedin\/answer\/4135?lang=en\'>Learn more about browsers you can use.<\/a>';}('__li__i18n_registry__'));</script>
                <script data-page-js-type='config'>(function(n,r,a){r=window[n]=window[n]||{};a=r['global:browserSupportPolicy']=r['global:browserSupportPolicy']||{};a['supportedBrowserMinVersions']={ie:'v10',firefox:'v38',opera:'control',safari:'v6.1',chrome:'v42',mobileSafari:'v7',android:'v2.3',androidChrome:'v0'};}('__li__config_registry__'));</script>
                <script type='text/javascript' src='https://static.licdn.com/scds/concat/common/js?h=3kp2aedn5pmamdr4dk4n8atur-3ti5bgrnb6idjtk0w4chaigxe-5hqr1i1uoezoj0z1s5gcxojf2-71o37tcjwl0ishto9izvyml3i-3bbdjshpw5ov0rwa8xe08tp97-cayct4cirf7n0f9z1xsg84g0q-dktkawxk7k8pixuh5g8z5ku32-213zbp2wzp99lviwl8g2cvq6i-1lknwtftishpdmobzm413yc7u-bcxa0v9ke411pjpmz4s239f9b-10wg3j2jlwnawjalr4lur4ho3-82rcsw42m1wbgsti4m3j0kvg6-f3la2n4kbk7vr56j54qax1oif-3rkdlr1xr4hx00ifvffl6snxh-8sox1gztdjnz2un89fi8fyw35-8hdbl769kuhp0h4bsexhsbks0-d8q8zy9bxe530nqv2fct70nwo-c6ct0moql4p4ngtzltmf8l3ly-8h514j3fiwnzuwkt66sbxsu8f-di2z9sra5co9la7ogqyesywin'></script>
                <script src='https://static.licdn.com/sc/h/f02eiiw8zlfbp5rcs1p4l4zi6' async></script>
                <script type='text/javascript'>if(!window.LI){window.LI={};}
                LI.RUM=LI.RUM||{};(function(RUM,win){var doc=win.document;RUM.flags=RUM.flags||{};RUM.flags['host-flag']='control';RUM.flags['pop_beacons_frequency']='n100-ap0-la0-va0-tx0-sg0-db0-hk0-sp0-ln0-ch0-sy0-mu0-mi0-llnw0-akam0-cdxscdn0-cdxssmall0-wwwsmall0';RUM.flags['rs_timings_individual']='n5000';RUM.flags['rs_timings_individual_detail']='enabled';RUM.urls=RUM.urls||{};RUM.urls['rum-track']='\/lite\/rum-track?csrfToken=ajax%3A2451404493697687063';RUM.urls['boomerang-bw-img']='https:\/\/static.licdn.com\/scds\/common\/u\/lib\/boomerang\/0.9.edge.4ab208445a\/img\/';RUM.base_urls=RUM.base_urls||{};RUM.base_urls['permanent_content']='https:\/\/static.licdn.com\/scds\/common\/u\/';RUM.base_urls['versioned_content']='https:\/\/static.licdn.com\/scds\/concat\/common\/';RUM.base_urls['media_proxy']='https:\/\/media.licdn.com\/media-proxy\/';RUM.serverStartTime=1.467133272735E12;RUM.enabled=true;function getRumScript(){var node=doc.body||doc.head||doc.getElementsByTagName('head')[0],script=doc.createElement('script');script.src=['https://static.licdn.com/scds/concat/common/js?h=ed29nkjpsa16bhrjq4na16owq-1mucgfycc664m7vmhpjgqse65-1l5rurej3h44qodo5rn0cdvyn-8om6v2ckrxsbnwf40t9ta8a7e-33fp3gmyxmfu3rd2ycin3twc0-34tiets5jpj294jd59h8c4s0n-28w7d5j2k2jtil9ncckolke4m-9jzlwicvu376y9q4vjq77y5ks-1m0whdrwis44c1hoa9mrwhlt4-1uvutm1mpyov7rqhtcf8fksby-aac54ic1fmca5xz1yvc5t9nfe-1hn40w0bomeivihj9lopp4hp2-c0121povror81d0xao0yez4gy'][0];node.appendChild(script);}
                if(win.addEventListener){win.addEventListener('load',getRumScript);}
                else{win.attachEvent('onload',getRumScript);}}(LI.RUM,window));</script>
                <meta name='detectAdBlock' content='//platform.linkedin.com/js/px.js'/>
                <script src='https://static.licdn.com/scds/concat/common/js?h=69w33ou4umkyupw2uqgn7za7w' async defer></script>
                <script class='comscore-tracking'>(function(_e,_r){var isNielsenDisabled=true;var providers={'COMSCORE':{'treatment':'control'}};if(!isNielsenDisabled){providers['NIELSEN']={'treatment':'control'};}
                var fireComscore=function(){var comScore=window.COMSCORE;if(comScore){comScore.beacon({c1:2,c2:6402952,c3:'',c4:'',c5:'',c6:'',c15:''});}};var uc=window.encodeURIComponent,timeStamp=(new Date()).getTime();var fireExternalTracking=function(et){if(typeof et.trackPageChromeInit==='function'){et.trackPageChromeInit(providers);}else{et.setTreatment('enabled_1.0');et.trackWithComScoreForChromeInit();if(!isNielsenDisabled){var img=new Image(1,1);img.onerror=img.onload=function(){img.onerror=img.onload=null;};img.src=['//secure-gl.imrworldwide.com/cgi-bin/m?ci=au-linkedin&cc=1&si=',uc(window.location.href),'&rp=',uc(document.referrer),'&ts=compact&rnd=',timeStamp].join('');}}};var track=function(){var et;if(_e){fireExternalTracking(_e);}else if(_r&&typeof _r.ensure==='function'){try{_r.ensure(['externalTracking'],function(require){try{et=require('externalTracking');fireExternalTracking(et);}catch(e){fireComscore();}});}catch(e){fireComscore();}}else if(_r&&_r._is_li){try{et=require('externalTracking');fireExternalTracking(et);}catch(e){fireComscore();}}else{fireComscore();}};window.addEventListener('load',track);}(window.externalTracking,window.require));</script>
                <noscript>
                <img src='https://sb.scorecardresearch.com/b?c1=2&amp;c2=6402952&amp;c3=&amp;c4=&amp;c5=&amp;c6=&amp;c15=&amp;cv=1.3&amp;cj=1' style='display:none' width='0' height='0' alt=''/>
                </noscript>
                <script type='text/javascript'>(function(d){function go(){var a=d.createElement('iframe');a.style.display='none';a.setAttribute('sandbox','allow-scripts allow-same-origin');a.src='//radar.cedexis.com/1/11326/radar/radar.html';if(d.body){d.body.appendChild(a);}}
                if(window.addEventListener){window.addEventListener('load',go,false);}else if(window.addEvent){window.addEvent('onload',go);}}(document));</script>
                </div>
                </body>
                </html>
                ";
            return html;
        }
        #endregion

        #region Normal Input
        [TestMethod]
        public void NormalInput_SimpleValuesExtract()
        {
            // Arrange
            const string url = "https://il.linkedin.com/in/talbronfer";

            // Act
            var result = _profileProvider.GetProfile(url);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(url, result.ProfileUrl);
            Assert.AreEqual("Tal Bronfer", result.Name);
            Assert.AreEqual("Software Developer at Gartner Innovation Center", result.CurrentTitle);
            Assert.AreEqual("Gartner", result.CurrentPosition);
            Assert.AreEqual("Full-stack developer specializing in Web Apps. Keen to learn, well-motivated and passionate about technology, entrepreneurship and innovation.", result.Summary);
            Assert.AreEqual("Israel", result.Location);
        }

        [TestMethod]
        public void NormalInput_SkillsExtract()
        {
            // Arrange
            const string url = "https://il.linkedin.com/in/talbronfer";
            var skills = new List<string>()
            {
                "AngularJS", "Node.js", "C#", ".NET", "MongoDB", "jQuery", "ASP.NET MVC", "ASP .NET Web API", "Web Applications", "JavaScript", "SQL", "Selenium", "Web Development"
            };

            // Act
            var result = _profileProvider.GetProfile(url);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(skills.Count, result.Skills.Count);
            Assert.IsTrue(skills.All(result.Skills.Contains));
            Assert.IsTrue(result.Skills.All(skills.Contains));
        }

        [TestMethod]
        public void NormalInput_GroupsExtract()
        {
            // Arrange
            const string url = "https://il.linkedin.com/in/talbronfer";
            var groups = new List<string>()
            {
                ".NET Experts in Israel", "Israel High Tech ++", "C# Developers / Architects", "EcoMotion Israel", "Test Automation", "Developers Network in Israel"
            };

            // Act
            var result = _profileProvider.GetProfile(url);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(groups.Count, result.Groups.Count);
            Assert.IsTrue(groups.All(result.Groups.Contains));
            Assert.IsTrue(result.Groups.All(groups.Contains));
        }

        [TestMethod]
        public void NormalInput_LanguagesExtract()
        {
            // Arrange
            const string url = "https://il.linkedin.com/in/talbronfer";
            var languages = new List<string>()
            {
                "Hebrew", "English"
            };

            // Act
            var result = _profileProvider.GetProfile(url);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(languages.Count, result.Languages.Count);
            Assert.IsTrue(languages.All(result.Languages.Contains));
            Assert.IsTrue(result.Languages.All(languages.Contains));
        }

        [TestMethod]
        public void NormalInput_RecommendationsExtract()
        {
            // Arrange
            const string url = "https://il.linkedin.com/in/talbronfer";
            const string recommendation =
                @"Tal is a talented full-stack developer. I had the opportunity to work with Tal on a couple of projects where he had full responsibility over the client-side architecture (using Angular.js at all times) and each time he got it way over every expectations.
                Great to work with!";

            // Act
            var result = _profileProvider.GetProfile(url);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Recommendations.Count);
            Assert.AreEqual(recommendation, result.Recommendations.First());
        }

        [TestMethod]
        public void NormalInput_AssociatedPeopleExtract()
        {
            // Arrange
            const string url = "https://il.linkedin.com/in/talbronfer";
            var firstPerson = new AssociatedPerson()
            {
                Name = "Daniel Goltz",
                Title = "Full-Stack .NET Developer",
                ProfileUrl = "https://il.linkedin.com/in/degoltz?trk=pub-pbmap"
            };
            var lastPerson = new AssociatedPerson()
            {
                Name = "Roie Labes",
                Title = "Software Programmer and Team Leader",
                ProfileUrl = "https://il.linkedin.com/in/roie-labes-913a19b7?trk=pub-pbmap"
            };

            // Act
            var result = _profileProvider.GetProfile(url);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.AssociatedPeople.Count);
            Assert.AreEqual(10, result.AssociatedPeople.Select(p => p.ProfileUrl).Distinct().Count());
            Assert.AreEqual(firstPerson.Name, result.AssociatedPeople.First().Name);
            Assert.AreEqual(firstPerson.Title, result.AssociatedPeople.First().Title);
            Assert.AreEqual(firstPerson.ProfileUrl, result.AssociatedPeople.First().ProfileUrl);
            Assert.AreEqual(lastPerson.Name, result.AssociatedPeople.Last().Name);
            Assert.AreEqual(lastPerson.Title, result.AssociatedPeople.Last().Title);
            Assert.AreEqual(lastPerson.ProfileUrl, result.AssociatedPeople.Last().ProfileUrl);
        }

        [TestMethod]
        public void NormalInput_EducationExtract()
        {
            // Arrange
            const string url = "https://il.linkedin.com/in/talbronfer";
            var firstEducation = new EducationInformation()
            {
                Institution = "the Open University of Israel",
                Degree = "Bachelor of Science (BSc), Applied Science",
                StartDate = "2012",
                EndDate = "2017",
            };
            var lastEducation = new EducationInformation()
            {
                Institution = "The Israel Arts and Science Academy",
                Degree = "High School, Natural Sciences",
                StartDate = "2007",
                EndDate = "2010",
                Description = "The Israel Arts and Science Academy (IASA) is a high school in Jerusalem for gifted students from all over Israel. Majored in computer science and biology."
            };

            // Act
            var result = _profileProvider.GetProfile(url);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Education.Count);
            Assert.AreEqual(firstEducation.Institution, result.Education.First().Institution);
            Assert.AreEqual(firstEducation.Degree, result.Education.First().Degree);
            Assert.AreEqual(firstEducation.StartDate, result.Education.First().StartDate);
            Assert.AreEqual(firstEducation.EndDate, result.Education.First().EndDate);
            Assert.AreEqual(firstEducation.Description, result.Education.First().Description);
            Assert.AreEqual(lastEducation.Institution, result.Education.Last().Institution);
            Assert.AreEqual(lastEducation.Degree, result.Education.Last().Degree);
            Assert.AreEqual(lastEducation.StartDate, result.Education.Last().StartDate);
            Assert.AreEqual(lastEducation.EndDate, result.Education.Last().EndDate);
            Assert.AreEqual(lastEducation.Description, result.Education.Last().Description);
        }

        [TestMethod]
        public void NormalInput_ExperienceExtract()
        {
            // Arrange
            const string url = "https://il.linkedin.com/in/talbronfer";
            var works = new List<WorkInformation>
            {
                #region works
                new WorkInformation()
                {
                    Title = "Software Developer",
                    Company = "Gartner",
                    StartDate = "May 2016",
                    EndDate = "Present"
                },
                new WorkInformation()
                {
                    Title = "Software Developer",
                    Company = "SafeBreach",
                    StartDate = "May 2015",
                    EndDate = "May 2016",
                    Description = @"- Designing & implementing Cloud-based REST APIs for the company's on-premise security solution using Node.js, MongoDB and the Express & Swagger frameworks.- Working with the company's UX department on developing the product's customer-facing UI using Angular.js. This includes dealing with many challenges in the modern SPA world, including authentication, communication with multiple external APIs, cross-device responsiveness & client-side performance.- Serving as the Point of Contact for the company's production Cloud environment, including responsibility for Continuous Integration & Delivery solutions, redundancy, applicative security and other aspects.- Working with BDD tests written in Yadda (Karma) and with JS task frameworks such as Gulp."
                },
                new WorkInformation()
                {
                    Title = "Co-founder",
                    Company = "Fullr",
                    StartDate = "December 2014",
                    EndDate = "April 2015",
                    Description = @"Fullr is creating a smart Connected Car device and a mobile application, making car ownership easier and cheaper by managing all car-related expenses in a personalized, effortless manner."
                },
                 new WorkInformation()
                {
                    Title = "Full-Stack Software Developer",
                    Company = "Israel Defense Forces",
                    StartDate = "January 2014",
                    EndDate = "January 2015",
                    Description = @"- Designing and developing Single Page Applications in an R&D team specializing in rapid, Agile-based implementation of new concepts- Full stack responsibility for undertaken projects, implementing server-side in .NET and client-side in AngularJS, jQuery and various other frameworks"
                },
                new WorkInformation()
                {
                    Title = "Software Automation Engineer",
                    Company = "Israel Defense Forces",
                    StartDate = "February 2013",
                    EndDate = "March 2014",
                    Location = "Israel",
                    Description = @"- Designed & developed continuously-integrating software test automation infrastructures and solutions for enterprise-scale .NET and Web-based systems deployed to thousands of users- Responsible for professional management and guidance of a team of several software test automation developers working with the above-mentioned infrastructures.- Researching and implementing new automated testing tools & methodologies in the organization."
                },
                new WorkInformation()
                {
                    Title = "Software QA Engineer",
                    Company = "Israel Defense Forces",
                    StartDate = "April 2011",
                    EndDate = "February 2013",
                    Location = "Israel",
                    Description = @"- Designed testing methodologies in user-oriented corporate software systems. - Designed & developed software tools for automation of various QA processes, including configuration and deployment management, production data analysis and testing in production. - Responsible for detailing customer business processes into requirements and development tasks.- Performed testing cycles while debugging, analyzing and prioritizing defects."
                },
                new WorkInformation()
                {
                    Title = "Contributing Writer",
                    Company = "TheTruthAboutCars.com",
                    StartDate = "December 2009",
                    EndDate = "December 2012",
                    Description = @"- Responsible for covering & providing in-depth analysis of the Better Place project - Conducted road tests and wrote reviews of various cars"
                }
                #endregion
            };


            // Act
            var result = _profileProvider.GetProfile(url);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(works.Count, result.Experience.Count);
            for (int i = 0; i < works.Count; i++)
            {
                Assert.AreEqual(works[i].Company, result.Experience[i].Company);
                Assert.AreEqual(works[i].Description, result.Experience[i].Description);
                Assert.AreEqual(works[i].EndDate, result.Experience[i].EndDate);
                Assert.AreEqual(works[i].Location, result.Experience[i].Location);
                Assert.AreEqual(works[i].StartDate, result.Experience[i].StartDate);
                Assert.AreEqual(works[i].Title, result.Experience[i].Title);
            }
        }

        [TestMethod]
        public void NormalInput_VolunteerExtract()
        {
            // Arrange
            const string url = "https://il.linkedin.com/in/talbronfer";
            var volunteerInformations = new List<VolunteerInformation>
            {
                #region volunteer
                new VolunteerInformation()
                {
                    Role = "Co-Organizer",
                    Organization = "Geekim Hackathon",
                    Cause = "Education",
                    StartDate = "November 2014",
                    EndDate = "Present",
                    Description = @"Co-organized the Geekim Hackathon, the first hackathon intended for active servants and alumni of the Intelligence Corps, where over a 100 participants helped develop apps and software solutions to technologically help and solve causes in the Israeli society, such as accessibility for the disabled, poverty, environment and others."
                },
                new VolunteerInformation()
                {
                    Role = "Training Officer",
                    Organization = "Israel Defense Forces",
                    Cause = "Education",
                    StartDate = "March 2014",
                    EndDate = "Present",
                    Description = @"Served twice as a training officer and later a point of contact for the Intelligence Corps' primary software development course, Mamas. Responsible for researching and developing lessons and exercises in the .NET, Web and Software Automation worlds, as well as developing the course's management system and other technological aides."
                },
                new VolunteerInformation()
                {
                    Role = "Co-Organizer",
                    Organization = "Geekim Hackathon",
                    Cause = "Education",
                    StartDate = "November 2015",
                    EndDate = "Present",
                    Description = @"Co-organized the 2015 Geekim Hackathon held for the second consecutive year. Responsible for raising funds, recruiting sponsors from the Israeli startup community, creating the judging format for the event and managing public relations."
                },
                new VolunteerInformation()
                {
                    Role = "Group Mentor",
                    Organization = "Magshimim",
                    Cause = "Education",
                    StartDate = "November 2015",
                    EndDate = "Present",
                    Description = @"The Magshimim initiative, led by the Israeli Ministry of Defense and the Rashi Foundation, is a practical program teaching excelling high school students cyber-security related subjects, including software development & networks. As a Group Mentor, I help a group of students with planning & executing their year-long final project, in both technical and business-related domains."
                }
                #endregion
            };


            // Act
            var result = _profileProvider.GetProfile(url);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(volunteerInformations.Count, result.Volunteer.Count);
            for (int i = 0; i < volunteerInformations.Count; i++)
            {
                Assert.AreEqual(volunteerInformations[i].Role, result.Volunteer[i].Role);
                Assert.AreEqual(volunteerInformations[i].Organization, result.Volunteer[i].Organization);
                Assert.AreEqual(volunteerInformations[i].Cause, result.Volunteer[i].Cause);
                Assert.AreEqual(volunteerInformations[i].StartDate, result.Volunteer[i].StartDate);
                Assert.AreEqual(volunteerInformations[i].EndDate, result.Volunteer[i].EndDate);
                Assert.AreEqual(volunteerInformations[i].Description, result.Volunteer[i].Description);
            }
        }
        #endregion

        #region Bad Input
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "An empty html was inappropriately allowed.")]
        public void EmptyInput_ExceptionThrown()
        {
            // Arrange
            const string url = "empty";

            // Act
            _profileProvider.GetProfile(url);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "An invalid html was inappropriately allowed.")]
        public void InvalidInput_ExceptionThrown()
        {
            // Arrange
            const string url = "invalid";

            // Act
             _profileProvider.GetProfile(url);
        }
        #endregion
    }


}
