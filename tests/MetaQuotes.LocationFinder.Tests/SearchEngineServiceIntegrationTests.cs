using MetaQuotes.LocationFinder.Core.Helpers;
using MetaQuotes.LocationFinder.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace MetaQuotes.LocationFinder.Tests
{
    public class SearchEngineFixture
    {
        public SearchEngineService SearchEngine { get; }

        public SearchEngineFixture()
        {
            var stubLogger = Mock.Of<ILogger<SearchEngineService>>();
            var searchIndex = DbReaderHelper.CreateSearchIndex("Data/geobase.dat");

            SearchEngine = new SearchEngineService(
                searchIndex,
                stubLogger);
        }
    }

    public class SearchEngineServiceIntegrationTests : IClassFixture<SearchEngineFixture>
    {
        private SearchEngineService _searchEngineService;

        public SearchEngineServiceIntegrationTests(SearchEngineFixture searchEngineFixture)
        {
            _searchEngineService = searchEngineFixture.SearchEngine;
        }

        [Theory]
        [InlineData("155.228.48.55", "cit_Ibesex Fe", "org_Ubez Gi Ofo ", "cou_OMO", "reg_Aho", "pos_89041", -140.1777f, 18.6284f)]
        [InlineData("32.132.49.55", "cit_Ibesex Fe", "org_Ubez Gi Ofo ", "cou_OMO", "reg_Aho", "pos_89041", -140.1777f, 18.6284f)]
        [InlineData("220.141.20.205", "cit_Ytuluju U", "org_Ida Uzonaq Ba Owop F", "cou_AJ", "reg_I ", "pos_97163", -155.4074f, -166.7253f)]
        [InlineData("160.191.20.205", "cit_Ytuluju U", "org_Ida Uzonaq Ba Owop F", "cou_AJ", "reg_I ", "pos_97163", -155.4074f, -166.7253f)]
        [InlineData("212.160.237.224", "cit_Ybasycadabigyw", "org_Erimo", "cou_ORU", "reg_An G", "pos_90508", -33.3477f, -150.6189f)]
        [InlineData("173.245.238.224", "cit_Ybasycadabigyw", "org_Erimo", "cou_ORU", "reg_An G", "pos_90508", -33.3477f, -150.6189f)]
        [InlineData("192.249.201.82", "cit_Uwaha", "org_U Exefalod Tawa", "cou_AB", "reg_Ukoxam", "pos_2941261", -16.3405f, 14.5992f)]
        [InlineData("99.141.202.82", "cit_Uwaha", "org_U Exefalod Tawa", "cou_AB", "reg_Ukoxam", "pos_2941261", -16.3405f, 14.5992f)]
        [InlineData("25.213.253.204", "cit_Ibopuvyvew", "org_Ezuhoqe I", "cou_OPA", "reg_Elup", "pos_0175070", -148.0829f, -71.2928f)]
        [InlineData("242.123.254.204", "cit_Ibopuvyvew", "org_Ezuhoqe I", "cou_OPA", "reg_Elup", "pos_0175070", -148.0829f, -71.2928f)]
        [InlineData("22.237.112.148", "cit_Ixam Do Alokeg", "org_Ol Micime I Awocipujuger", "cou_OX", "reg_Omiwaw", "pos_187555", 74.4023f, 159.2652f)]
        [InlineData("191.34.114.148", "cit_Ixam Do Alokeg", "org_Ol Micime I Awocipujuger", "cou_OX", "reg_Omiwaw", "pos_187555", 74.4023f, 159.2652f)]
        [InlineData("137.153.208.208", "cit_Ace U Or Pi", "org_Ufeq", "cou_IB", "reg_Y Iza", "pos_95070", -3.0386f, 107.8946f)]
        [InlineData("223.187.209.208", "cit_Ace U Or Pi", "org_Ufeq", "cou_IB", "reg_Y Iza", "pos_95070", -3.0386f, 107.8946f)]
        [InlineData("148.164.207.177", "cit_I O Op", "org_Anona Etyhitujicez", "cou_UB", "reg_O Afy", "pos_7947", 171.6118f, 157.4118f)]
        [InlineData("40.91.12.167", "cit_Ylalu U", "org_Ix Hiti Iqe ", "cou_ERO", "reg_Ewe", "pos_805810", 94.4341f, 162.472f)]
        [InlineData("8.86.13.167", "cit_Ylalu U", "org_Ix Hiti Iqe ", "cou_ERO", "reg_Ewe", "pos_805810", 94.4341f, 162.472f)]
        [InlineData("105.228.91.76", "cit_Idusikafil De Ufaw", "org_Ojikonyqu Opafu Erejat Si", "cou_IS", "reg_Oki", "pos_683339", -109.3408f, 173.8303f)]
        [InlineData("148.10.92.76", "cit_Idusikafil De Ufaw", "org_Ojikonyqu Opafu Erejat Si", "cou_IS", "reg_Oki", "pos_683339", -109.3408f, 173.8303f)]
        [InlineData("154.50.42.6", "cit_Er Rage Abexajopaz", "org_Udi Yx Bodahoq Ki I Af", "cou_YH", "reg_Ik", "pos_8509", 76.9303f, 97.8391f)]
        [InlineData("143.234.42.6", "cit_Er Rage Abexajopaz", "org_Udi Yx Bodahoq Ki I Af", "cou_YH", "reg_Ik", "pos_8509", 76.9303f, 97.8391f)]
        [InlineData("1.22.156.184", "cit_Yxanyki Ufafel ", "org_E Ymav ", "cou_YS", "reg_Yw", "pos_2715087", -110.6781f, -84.5955f)]
        [InlineData("239.68.156.184", "cit_Yxanyki Ufafel ", "org_E Ymav ", "cou_YS", "reg_Yw", "pos_2715087", -110.6781f, -84.5955f)]
        [InlineData("33.134.177.39", "cit_Imoja Y Upuqade", "org_Ysexy Akofa", "cou_YH", "reg_Uwas", "pos_0826", -126.4118f, 16.7542f)]
        [InlineData("12.15.178.39", "cit_Imoja Y Upuqade", "org_Ysexy Akofa", "cou_YH", "reg_Uwas", "pos_0826", -126.4118f, 16.7542f)]
        [InlineData("174.33.237.211", "cit_I Yfuqahil Hehela", "org_Esiv Kekysibituz", "cou_ER", "reg_Ekuq", "pos_2889098", 27.416f, 116.2554f)]
        [InlineData("26.246.237.211", "cit_I Yfuqahil Hehela", "org_Esiv Kekysibituz", "cou_ER", "reg_Ekuq", "pos_2889098", 27.416f, 116.2554f)]
        [InlineData("186.179.108.154", "cit_Awamopazy I Eqabab", "org_Etag P Dyqej", "cou_UM", "reg_O", "pos_710454", 89.3967f, 63.3247f)]
        [InlineData("108.25.110.154", "cit_Awamopazy I Eqabab", "org_Etag P Dyqej", "cou_UM", "reg_O", "pos_710454", 89.3967f, 63.3247f)]
        [InlineData("13.11.203.154", "cit_Ugi Yre Edo", "org_Ypezanimeni Ofaredyzynip", "cou_UM", "reg_Ec", "pos_68225", -155.0473f, -9.9334f)]
        [InlineData("10.89.60.60", "cit_Izu", "org_Ujepawipabezoc", "cou_EG", "reg_Ulaw ", "pos_50085", -88.0481f, -111.4177f)]
        [InlineData("50.49.61.60", "cit_Izu", "org_Ujepawipabezoc", "cou_EG", "reg_Ulaw ", "pos_50085", -88.0481f, -111.4177f)]
        [InlineData("65.118.186.95", "cit_Iwab Vyqu Eh Pi", "org_O Yxe Obi Oxu", "cou_AF", "reg_Oxa", "pos_3249201", 79.2629f, 7.8646f)]
        [InlineData("162.219.187.95", "cit_Iwab Vyqu Eh Pi", "org_O Yxe Obi Oxu", "cou_AF", "reg_Oxa", "pos_3249201", 79.2629f, 7.8646f)]
        [InlineData("92.182.98.224", "cit_Anyly", "org_Uhikof Ty", "cou_EV", "reg_A", "pos_4537", 120.2604f, 102.6927f)]
        [InlineData("114.194.98.224", "cit_Anyly", "org_Uhikof Ty", "cou_EV", "reg_A", "pos_4537", 120.2604f, 102.6927f)]
        [InlineData("96.215.196.99", "cit_O E", "org_I Omaxupexuz Mu U Awaka", "cou_AT", "reg_E", "pos_5497603", -35.3794f, 135.5203f)]
        [InlineData("122.235.196.99", "cit_O E", "org_I Omaxupexuz Mu U Awaka", "cou_AT", "reg_E", "pos_5497603", -35.3794f, 135.5203f)]
        [InlineData("118.46.32.13", "cit_Ydajaq Wesilas", "org_U Ah Nacekiwykoq ", "cou_OCE", "reg_E", "pos_9853", 28.0494f, 40.2848f)]
        [InlineData("23.226.32.13", "cit_Ydajaq Wesilas", "org_U Ah Nacekiwykoq ", "cou_OCE", "reg_E", "pos_9853", 28.0494f, 40.2848f)]
        [InlineData("220.31.105.71", "cit_Eseriv Ga A ", "org_O Acopajesu Y O Upypo U", "cou_ER", "reg_Up", "pos_810778", -66.6469f, -114.3238f)]
        [InlineData("83.238.105.71", "cit_Eseriv Ga A ", "org_O Acopajesu Y O Upypo U", "cou_ER", "reg_Up", "pos_810778", -66.6469f, -114.3238f)]
        [InlineData("246.146.124.225", "cit_Uvel", "org_Oriwabijetivas M Wo", "cou_IME", "reg_Ygod", "pos_5876295", -143.6165f, -99.0545f)]
        [InlineData("144.160.124.225", "cit_Uvel", "org_Oriwabijetivas M Wo", "cou_IME", "reg_Ygod", "pos_5876295", -143.6165f, -99.0545f)]
        [InlineData("140.76.203.129", "cit_Ebocucusajeg Be O", "org_Yga Ocis Zukomyzyxekube", "cou_AJ", "reg_Y", "pos_853766", -60.6438f, 105.968f)]
        [InlineData("9.123.204.129", "cit_Ebocucusajeg Be O", "org_Yga Ocis Zukomyzyxekube", "cou_AJ", "reg_Y", "pos_853766", -60.6438f, 105.968f)]
        [InlineData("73.183.215.189", "cit_Ebiramehytoma", "org_I Ypevucapasejyre Emywo", "cou_YFO", "reg_Ah Quz", "pos_9683616", -148.1815f, -167.1815f)]
        [InlineData("84.209.216.189", "cit_Ebiramehytoma", "org_I Ypevucapasejyre Emywo", "cou_YFO", "reg_Ah Quz", "pos_9683616", -148.1815f, -167.1815f)]
        [InlineData("12.237.14.76", "cit_On Fileruzul Deg", "org_Iv Fu", "cou_ON", "reg_Eg", "pos_06756", 128.9621f, 134.7627f)]
        [InlineData("149.222.50.18", "cit_Ysabi E Oxo Aduse", "org_Ejetekite Egugotuvefyqa", "cou_YFO", "reg_Og W", "pos_83341", -15.1361f, 64.0951f)]
        [InlineData("144.109.0.212", "cit_Ypenixu E", "org_Ev Sujuruvyfutizahaxama Yl", "cou_YTY", "reg_I", "pos_374354", -100.6745f, 82.6627f)]
        [InlineData("146.182.0.212", "cit_Ypenixu E", "org_Ev Sujuruvyfutizahaxama Yl", "cou_YTY", "reg_I", "pos_374354", -100.6745f, 82.6627f)]
        [InlineData("208.14.224.38", "cit_O Ijekipa", "org_Odytyc", "cou_YQE", "reg_A Uz", "pos_4482", 69.3476f, 45.5584f)]
        [InlineData("182.77.97.118", "cit_Axavini Iqu", "org_U Yq Mib Kulecy Irucy ", "cou_IZ", "reg_Ir", "pos_64139", 4.813f, -101.1962f)]
        [InlineData("168.116.218.104", "cit_Ekexop Canov Gy", "org_Olyze", "cou_YJ", "reg_Aqi", "pos_14148", 102.7853f, 135.9418f)]
        [InlineData("217.13.202.181", "cit_I Ak", "org_Agejufavu", "cou_YH", "reg_Ajeb ", "pos_6240", 178.8168f, 132.1466f)]
        [InlineData("173.116.203.181", "cit_I Ak", "org_Agejufavu", "cou_YH", "reg_Ajeb ", "pos_6240", 178.8168f, 132.1466f)]
        [InlineData("217.237.11.171", "cit_Ygidu Anuq F Ja", "org_Ulovepez Narogujadiqo", "cou_EJ", "reg_Ak", "pos_45176", -173.7119f, 105.0811f)]
        [InlineData("192.113.12.171", "cit_Ygidu Anuq F Ja", "org_Ulovepez Narogujadiqo", "cou_EJ", "reg_Ak", "pos_45176", -173.7119f, 105.0811f)]
        [InlineData("217.3.33.69", "cit_Aw Supo El", "org_Amoba O U", "cou_AB", "reg_Uku", "pos_031175", -129.5474f, 95.2389f)]
        [InlineData("4.66.171.20", "cit_E Ucatifybotofic", "org_Ej Qep Tetas Bi", "cou_OH", "reg_Eb", "pos_203209", -15.3254f, 131.9913f)]
        [InlineData("135.23.172.20", "cit_E Ucatifybotofic", "org_Ej Qep Tetas Bi", "cou_OH", "reg_Eb", "pos_203209", -15.3254f, 131.9913f)]
        [InlineData("5.18.193.9", "cit_Izyj Dif", "org_Eze Izisu Ix Xomibive", "cou_IZ", "reg_Ovu Oq", "pos_991165", -97.5786f, 38.7514f)]
        [InlineData("42.142.193.9", "cit_Izyj Dif", "org_Eze Izisu Ix Xomibive", "cou_IZ", "reg_Ovu Oq", "pos_991165", -97.5786f, 38.7514f)]
        [InlineData("193.106.81.173", "cit_Ex Bonoliwabysihu", "org_O I Etupunana Eri", "cou_IT", "reg_U", "pos_00061", 72.4686f, -39.1075f)]
        [InlineData("224.123.82.173", "cit_Ex Bonoliwabysihu", "org_O I Etupunana Eri", "cou_IT", "reg_U", "pos_00061", 72.4686f, -39.1075f)]
        [InlineData("221.251.254.230", "cit_Ud", "org_Ewexyzalon ", "cou_IM", "reg_Yve", "pos_8914", -158.937f, -73.7106f)]
        [InlineData("140.245.189.98", "cit_Upo", "org_Ep W Wu", "cou_YKY", "reg_Adamo ", "pos_3239965", -127.2262f, -136.1594f)]
        [InlineData("40.83.191.98", "cit_Upo", "org_Ep W Wu", "cou_YKY", "reg_Adamo ", "pos_3239965", -127.2262f, -136.1594f)]
        [InlineData("52.171.10.173", "cit_Ywu Asy Ono I", "org_Ykeduziziwa Ujex Wate ", "cou_ORO", "reg_Yd Fuk", "pos_05914", -152.2285f, -118.1126f)]
        [InlineData("248.1.11.173", "cit_Ywu Asy Ono I", "org_Ykeduziziwa Ujex Wate ", "cou_ORO", "reg_Yd Fuk", "pos_05914", -152.2285f, -118.1126f)]
        [InlineData("224.166.128.102", "cit_Ihekata", "org_Yw F Ta", "cou_UB", "reg_U", "pos_338878", 124.6934f, 78.0914f)]
        [InlineData("50.34.150.29", "cit_Imiqyha", "org_Y Uhi ", "cou_IN", "reg_E", "pos_0991608", 20.1967f, 1.3089f)]
        [InlineData("214.47.151.29", "cit_Imiqyha", "org_Y Uhi ", "cou_IN", "reg_E", "pos_0991608", 20.1967f, 1.3089f)]
        [InlineData("213.136.1.10", "cit_Esynisuvafap Wob", "org_Abib Titel Japoramefu Yw", "cou_ILA", "reg_Iz", "pos_42950", 149.5399f, -149.7097f)]
        [InlineData("255.150.1.10", "cit_Esynisuvafap Wob", "org_Abib Titel Japoramefu Yw", "cou_ILA", "reg_Iz", "pos_42950", 149.5399f, -149.7097f)]
        [InlineData("21.220.166.193", "cit_O A Ymyrymo", "org_Ow Q Ham", "cou_AF", "reg_O Evu", "pos_99294", -172.1434f, -168.3957f)]
        [InlineData("84.227.167.193", "cit_O A Ymyrymo", "org_Ow Q Ham", "cou_AF", "reg_O Evu", "pos_99294", -172.1434f, -168.3957f)]
        [InlineData("248.106.99.180", "cit_U Uky Axefeqepobax", "org_Anujevedow Darabuv", "cou_OVO", "reg_Utite", "pos_5382556", 16.1103f, -162.2211f)]
        [InlineData("227.10.100.180", "cit_U Uky Axefeqepobax", "org_Anujevedow Darabuv", "cou_OVO", "reg_Utite", "pos_5382556", 16.1103f, -162.2211f)]
        [InlineData("216.203.207.43", "cit_Ofyhitux", "org_Yzecylybomy", "cou_EDE", "reg_Iw F", "pos_44403", -11.5803f, 9.0939f)]
        [InlineData("190.225.208.43", "cit_Ofyhitux", "org_Yzecylybomy", "cou_EDE", "reg_Iw F", "pos_44403", -11.5803f, 9.0939f)]
        [InlineData("188.69.33.89", "cit_Ih Zas Matyf", "org_E Yratoc Xah", "cou_IKU", "reg_Isynu", "pos_96813", -56.9786f, 154.1182f)]
        [InlineData("210.111.33.89", "cit_Ih Zas Matyf", "org_E Yratoc Xah", "cou_IKU", "reg_Isynu", "pos_96813", -56.9786f, 154.1182f)]
        [InlineData("37.216.164.34", "cit_Azo Yvavofenur", "org_Yt Me Em Q Ta", "cou_IME", "reg_Ec", "pos_611059", 70.5542f, -146.6111f)]
        [InlineData("179.251.164.34", "cit_Azo Yvavofenur", "org_Yt Me Em Q Ta", "cou_IME", "reg_Ec", "pos_611059", 70.5542f, -146.6111f)]
        [InlineData("61.201.149.26", "cit_Ibibu", "org_Ig Fejyjowo E Ibubes ", "cou_AF", "reg_Onop", "pos_36378", -152.7541f, 173.7903f)]
        [InlineData("192.221.149.26", "cit_Ibibu", "org_Ig Fejyjowo E Ibubes ", "cou_AF", "reg_Onop", "pos_36378", -152.7541f, 173.7903f)]
        [InlineData("23.178.0.27", "cit_Eqil", "org_Olexage Oqu Eti Uqutyx", "cou_IB", "reg_Y Aze", "pos_4700", -142.3704f, 27.5033f)]
        [InlineData("102.165.1.27", "cit_Eqil", "org_Olexage Oqu Eti Uqutyx", "cou_IB", "reg_Y Aze", "pos_4700", -142.3704f, 27.5033f)]
        [InlineData("158.220.4.105", "cit_Aqohe", "org_Iniky", "cou_YCU", "reg_O ", "pos_91432", -30.142f, -97.3232f)]
        [InlineData("27.209.240.2", "cit_Ydajaq Wesilas", "org_Ovy Eqo Uso Okasyke U", "cou_AV", "reg_Ec", "pos_6936209", 163.6679f, -153.5108f)]
        [InlineData("115.161.211.105", "cit_Esu Arixazanehe", "org_Ilevimuw Fada Elu Iwavoti", "cou_IKU", "reg_A", "pos_205082", -75.0172f, -168.4767f)]
        [InlineData("221.8.161.119", "cit_Ynac G Qeto Ober W", "org_Ytexesifob", "cou_OQA", "reg_Uzyb", "pos_9050148", 91.0724f, -20.4397f)]
        [InlineData("60.25.161.119", "cit_Ynac G Qeto Ober W", "org_Ytexesifob", "cou_OQA", "reg_Uzyb", "pos_9050148", 91.0724f, -20.4397f)]
        [InlineData("69.170.46.117", "cit_Ijejuba ", "org_Epuvelapemedep Naj", "cou_UCY", "reg_Y", "pos_4079", -152.1083f, 100.3929f)]
        [InlineData("125.41.47.117", "cit_Ijejuba ", "org_Epuvelapemedep Naj", "cou_UCY", "reg_Y", "pos_4079", -152.1083f, 100.3929f)]
        [InlineData("99.210.23.215", "cit_Adur Tozexo Ic ", "org_Ek G Voc Cyd", "cou_OMO", "reg_A", "pos_6709", 142.3073f, -74.3337f)]
        [InlineData("6.26.25.215", "cit_Adur Tozexo Ic ", "org_Ek G Voc Cyd", "cou_OMO", "reg_A", "pos_6709", 142.3073f, -74.3337f)]
        [InlineData("109.62.81.211", "cit_Alosagyg L", "org_Axopewu Yh", "cou_YSY", "reg_Uva", "pos_16786", 55.4198f, -68.08f)]
        [InlineData("191.136.82.211", "cit_Alosagyg L", "org_Axopewu Yh", "cou_YSY", "reg_Uva", "pos_16786", 55.4198f, -68.08f)]
        [InlineData("177.57.146.94", "cit_Apakare Ocytu I", "org_Al W Modab Fesi O ", "cou_YFO", "reg_Exoc", "pos_640680", -43.3109f, 155.2488f)]
        [InlineData("18.142.146.94", "cit_Apakare Ocytu I", "org_Al W Modab Fesi O ", "cou_YFO", "reg_Exoc", "pos_640680", -43.3109f, 155.2488f)]
        [InlineData("194.166.78.124", "cit_Uribykoba Ogeluli", "org_Izu Ifo Udexen", "cou_EFA", "reg_Ug", "pos_304038", -63.8089f, 85.747f)]
        [InlineData("224.254.79.124", "cit_Uribykoba Ogeluli", "org_Izu Ifo Udexen", "cou_EFA", "reg_Ug", "pos_304038", -63.8089f, 85.747f)]
        [InlineData("138.118.171.128", "cit_Acewyvehisehywyb", "org_Exum Qoda Elequm Vihi ", "cou_EJ", "reg_Ocu", "pos_1233", 150.9928f, -123.0898f)]
        [InlineData("237.114.172.128", "cit_Acewyvehisehywyb", "org_Exum Qoda Elequm Vihi ", "cou_EJ", "reg_Ocu", "pos_1233", 150.9928f, -123.0898f)]
        [InlineData("209.211.93.130", "cit_Uced M Fi", "org_Uda ", "cou_YWU", "reg_In", "pos_86707", -47.2984f, 50.6084f)]
        [InlineData("235.8.95.130", "cit_Uced M Fi", "org_Uda ", "cou_YWU", "reg_In", "pos_86707", -47.2984f, 50.6084f)]
        [InlineData("104.1.156.228", "cit_O Adofobyvitipyk", "org_A Oluqodyfev Reloqoxic", "cou_AF", "reg_Uhyte", "pos_5874", -91.1478f, -88.0092f)]
        [InlineData("79.78.157.228", "cit_O Adofobyvitipyk", "org_A Oluqodyfev Reloqoxic", "cou_AF", "reg_Uhyte", "pos_5874", -91.1478f, -88.0092f)]
        [InlineData("60.31.60.139", "cit_Eqil", "org_Eteky", "cou_YKE", "reg_Ecuto ", "pos_3104719", 12.3763f, -98.219f)]
        [InlineData("3.20.187.48", "cit_Ujocusovaz", "org_Or Qy", "cou_YNO", "reg_Uzyg B", "pos_9173617", 56.8769f, 69.646f)]
        [InlineData("85.32.0.203", "cit_Iwu", "org_Osapame Oliz Jikujyzojaxuj", "cou_UJO", "reg_Uva", "pos_5380", -85.1786f, 179.5935f)]
        [InlineData("183.82.1.203", "cit_Iwu", "org_Osapame Oliz Jikujyzojaxuj", "cou_UJO", "reg_Uva", "pos_5380", -85.1786f, 179.5935f)]
        [InlineData("123.132.10.56", "cit_Upixog Ryj Da U", "org_Iko Odat Du Uxes", "cou_OCE", "reg_Y", "pos_413409", 164.9213f, 99.885f)]
        [InlineData("155.229.11.56", "cit_Upixog Ryj Da U", "org_Iko Odat Du Uxes", "cou_OCE", "reg_Y", "pos_413409", 164.9213f, 99.885f)]
        [InlineData("209.252.140.13", "cit_Enebevenojabim Ja ", "org_Oqe Iwupenymek Juboduvyka", "cou_ER", "reg_Us", "pos_34788", -26.4036f, -122.8172f)]
        [InlineData("173.194.141.13", "cit_Enebevenojabim Ja ", "org_Oqe Iwupenymek Juboduvyka", "cou_ER", "reg_Us", "pos_34788", -26.4036f, -122.8172f)]
        [InlineData("142.156.5.202", "cit_Yl Ruxaxok", "org_Erytepibot Mora", "cou_UJ", "reg_Yx", "pos_417324", -133.2863f, -168.4492f)]
        [InlineData("18.31.6.202", "cit_Yl Ruxaxok", "org_Erytepibot Mora", "cou_UJ", "reg_Yx", "pos_417324", -133.2863f, -168.4492f)]
        [InlineData("202.222.225.190", "cit_Ofa", "org_Iwo", "cou_YS", "reg_Yki", "pos_948667", -150.3519f, 90.805f)]
        [InlineData("31.11.227.190", "cit_Ofa", "org_Iwo", "cou_YS", "reg_Yki", "pos_948667", -150.3519f, 90.805f)]
        [InlineData("203.130.194.144", "cit_Is Zeq Jojez", "org_Ebucaka Iluhowixagavy", "cou_IN", "reg_Oryk", "pos_43924", -93.9086f, -156.6382f)]
        [InlineData("52.168.195.144", "cit_Is Zeq Jojez", "org_Ebucaka Iluhowixagavy", "cou_IN", "reg_Oryk", "pos_43924", -93.9086f, -156.6382f)]
        [InlineData("178.136.52.213", "cit_Ykeqi", "org_Atitotowagaq", "cou_ERE", "reg_Owoba", "pos_100098", -135.2956f, -47.3919f)]
        [InlineData("6.230.52.213", "cit_Ykeqi", "org_Atitotowagaq", "cou_ERE", "reg_Owoba", "pos_100098", -135.2956f, -47.3919f)]
        [InlineData("206.25.246.60", "cit_Yg Sydedubabu U Ov", "org_Aducigeq", "cou_OZ", "reg_Eke", "pos_7254", 36.206f, -120.4259f)]
        [InlineData("23.64.246.60", "cit_Yg Sydedubabu U Ov", "org_Aducigeq", "cou_OZ", "reg_Eke", "pos_7254", 36.206f, -120.4259f)]
        [InlineData("83.210.181.228", "cit_Ilezypyvifileqiguc", "org_Ila", "cou_YXA", "reg_Op", "pos_5663", -63.6177f, 65.9666f)]
        [InlineData("158.37.183.228", "cit_Ilezypyvifileqiguc", "org_Ila", "cou_YXA", "reg_Op", "pos_5663", -63.6177f, 65.9666f)]
        [InlineData("30.122.23.51", "cit_Ilege Uvad Le", "org_Oqylequxikirohacel", "cou_USA", "reg_U", "pos_6216", 25.4663f, 74.0594f)]
        [InlineData("67.3.24.51", "cit_Ilege Uvad Le", "org_Oqylequxikirohacel", "cou_USA", "reg_U", "pos_6216", 25.4663f, 74.0594f)]
        [InlineData("64.8.255.228", "cit_Yqigasiga Yvalaju", "org_I Az ", "cou_YCU", "reg_Alimos", "pos_27181", -91.7403f, 68.573f)]
        [InlineData("189.202.255.228", "cit_Yqigasiga Yvalaju", "org_I Az ", "cou_YCU", "reg_Alimos", "pos_27181", -91.7403f, 68.573f)]
        [InlineData("109.58.33.215", "cit_Ime", "org_Ajiziqofuven Tatawyr", "cou_YJ", "reg_Eqeci", "pos_8890665", -168.5242f, 39.0412f)]
        [InlineData("141.89.181.61", "cit_Yr Xaha Ufeda", "org_Enahakevo", "cou_UM", "reg_In", "pos_4656417", 153.6918f, -56.0769f)]
        [InlineData("69.157.181.61", "cit_Yr Xaha Ufeda", "org_Enahakevo", "cou_UM", "reg_In", "pos_4656417", 153.6918f, -56.0769f)]
        [InlineData("186.186.172.31", "cit_Ezare Apit D", "org_Edysenakameqage", "cou_AGU", "reg_U", "pos_5103982", -33.9137f, -86.101f)]
        [InlineData("53.59.173.31", "cit_Ezare Apit D", "org_Edysenakameqage", "cou_AGU", "reg_U", "pos_5103982", -33.9137f, -86.101f)]
        [InlineData("183.50.183.180", "cit_Ywab Poco", "org_E Oteg ", "cou_AB", "reg_Ebanak", "pos_1574", -137.6735f, -107.1127f)]
        [InlineData("184.0.184.180", "cit_Ywab Poco", "org_E Oteg ", "cou_AB", "reg_Ebanak", "pos_1574", -137.6735f, -107.1127f)]
        [InlineData("42.33.9.146", "cit_Ekad Jivetapeby", "org_Eduziva Ubedote", "cou_YQE", "reg_Yn J", "pos_20395", -4.5692f, -8.2302f)]
        [InlineData("129.69.10.146", "cit_Ekad Jivetapeby", "org_Eduziva Ubedote", "cou_YQE", "reg_Yn J", "pos_20395", -4.5692f, -8.2302f)]
        [InlineData("28.136.112.218", "cit_Ugyzu Ed", "org_Ytatac ", "cou_ESI", "reg_Y U", "pos_1025708", -38.2363f, 127.8873f)]
        [InlineData("132.156.113.218", "cit_Ugyzu Ed", "org_Ytatac ", "cou_ESI", "reg_Y U", "pos_1025708", -38.2363f, 127.8873f)]
        [InlineData("253.86.67.215", "cit_Ybewe Ala Omaz", "org_Ybov F F", "cou_IFY", "reg_Aja A", "pos_97457", -109.6189f, 81.3648f)]
        [InlineData("31.42.49.185", "cit_Y Idajojezagaberab", "org_Itoro ", "cou_IKU", "reg_Yjuhyh", "pos_2990821", 11.666f, 178.7335f)]
        [InlineData("112.10.220.11", "cit_O Ac N", "org_Esusoqo Obirohew", "cou_IKU", "reg_Omeka", "pos_5744", 157.045f, 163.0887f)]
        [InlineData("9.22.166.13", "cit_Ujymudahucabiwoh ", "org_Od Q Qec Zocozamenaxo", "cou_YQE", "reg_Opiva", "pos_5551", -135.3221f, 31.1028f)]
        [InlineData("66.244.186.101", "cit_Era Ujuq Tysigi O", "org_Orelek Go", "cou_AJ", "reg_Edul", "pos_5584363", -85.7218f, 30.954f)]
        [InlineData("137.77.188.101", "cit_Era Ujuq Tysigi O", "org_Orelek Go", "cou_AJ", "reg_Edul", "pos_5584363", -85.7218f, 30.954f)]
        [InlineData("113.247.220.75", "cit_Umim Zedefyqy Ucyn", "org_A Exeluryc N Te Ycupu ", "cou_UPA", "reg_Uhupy ", "pos_03048", 9.1275f, -72.8525f)]
        [InlineData("129.87.221.75", "cit_Umim Zedefyqy Ucyn", "org_A Exeluryc N Te Ycupu ", "cou_UPA", "reg_Uhupy ", "pos_03048", 9.1275f, -72.8525f)]
        [InlineData("67.74.151.51", "cit_Uwagycyguwyn", "org_Izemidaviz Fobuz", "cou_IKU", "reg_Y", "pos_568122", 144.7523f, -37.3088f)]
        [InlineData("170.168.151.51", "cit_Uwagycyguwyn", "org_Izemidaviz Fobuz", "cou_IKU", "reg_Y", "pos_568122", 144.7523f, -37.3088f)]
        [InlineData("200.68.221.182", "cit_Ar", "org_Oquvoduvek Mal ", "cou_ED", "reg_Ipedi", "pos_6981801", -63.1731f, 80.342f)]
        [InlineData("49.236.221.182", "cit_Ar", "org_Oquvoduvek Mal ", "cou_ED", "reg_Ipedi", "pos_6981801", -63.1731f, 80.342f)]
        [InlineData("168.196.88.37", "cit_Ojaketukyga ", "org_Oqixonomopegy", "cou_OZ", "reg_Yt Ke ", "pos_74064", -50.5331f, 114.297f)]
        [InlineData("155.234.89.37", "cit_Ojaketukyga ", "org_Oqixonomopegy", "cou_OZ", "reg_Yt Ke ", "pos_74064", -50.5331f, 114.297f)]
        [InlineData("195.185.17.30", "cit_Ah Cexa", "org_Ixer", "cou_ERE", "reg_Ewe", "pos_6831", 146.6399f, -3.1487f)]
        [InlineData("56.146.18.30", "cit_Ah Cexa", "org_Ixer", "cou_ERE", "reg_Ewe", "pos_6831", 146.6399f, -3.1487f)]
        [InlineData("187.72.146.78", "cit_Aq Zobu ", "org_A Ydanymag", "cou_UXU", "reg_Ojafab", "pos_34521", 130.1993f, 73.0434f)]
        [InlineData("183.232.25.106", "cit_Azaquv", "org_I Ubesurac Nebyciwo Ixedu", "cou_ERE", "reg_Igin", "pos_263270", -134.3358f, 80.9329f)]
        [InlineData("187.56.171.201", "cit_Yk", "org_Awoxujepiravid Zelub Ha", "cou_IS", "reg_Oma", "pos_144356", -95.3058f, 166.852f)]
        [InlineData("52.152.172.201", "cit_Yk", "org_Awoxujepiravid Zelub Ha", "cou_IS", "reg_Oma", "pos_144356", -95.3058f, 166.852f)]
        [InlineData("126.132.218.42", "cit_Yziqefopy ", "org_Emojacada Epat Kazavev", "cou_OVO", "reg_I Ot ", "pos_9575", -44.2895f, -177.3372f)]
        [InlineData("15.78.172.105", "cit_Isadidenudi I", "org_Uxifemowu Ajecigedut", "cou_ED", "reg_Yz", "pos_7561", -137.4881f, -86.4964f)]
        [InlineData("118.188.172.105", "cit_Isadidenudi I", "org_Uxifemowu Ajecigedut", "cou_ED", "reg_Yz", "pos_7561", -137.4881f, -86.4964f)]
        [InlineData("161.205.115.121", "cit_Ede Edyqa Ad ", "org_Ohat J", "cou_YXA", "reg_Osew", "pos_2951", 101.8944f, -2.2859f)]
        [InlineData("185.254.116.121", "cit_Ede Edyqa Ad ", "org_Ohat J", "cou_YXA", "reg_Osew", "pos_2951", 101.8944f, -2.2859f)]
        [InlineData("234.76.203.192", "cit_Uloxih Ku Ebelod", "org_Ot Vojas Pozimofusevicu", "cou_EG", "reg_Aka", "pos_9908792", 76.245f, 107.8757f)]
        [InlineData("39.41.204.192", "cit_Uloxih Ku Ebelod", "org_Ot Vojas Pozimofusevicu", "cou_EG", "reg_Aka", "pos_9908792", 76.245f, 107.8757f)]
        [InlineData("141.108.95.222", "cit_Eqonavi Eki ", "org_Exyt Vavyryg Gesiv", "cou_ER", "reg_Y ", "pos_5189", 35.7496f, 2.7627f)]
        [InlineData("3.125.96.222", "cit_Eqonavi Eki ", "org_Exyt Vavyryg Gesiv", "cou_ER", "reg_Y ", "pos_5189", 35.7496f, 2.7627f)]
        [InlineData("224.62.161.124", "cit_Ivybe Ocejufyvu", "org_Yvyf Bazac", "cou_OW", "reg_Omeka", "pos_0689697", -78.0823f, -38.8905f)]
        [InlineData("225.89.161.124", "cit_Ivybe Ocejufyvu", "org_Yvyf Bazac", "cou_OW", "reg_Omeka", "pos_0689697", -78.0823f, -38.8905f)]
        public void FindLocationByIp_OrdinalSearch_ReturnsExpected(
            string ip, 
            string city, 
            string org, 
            string country, 
            string region, 
            string postal, 
            float latitude, 
            float longitude)
        {
            // Arrange

            // Act
            var location = _searchEngineService.FindLocationByIp(ip);

            // Assert
            Assert.Equal(city, location.City);
            Assert.Equal(org, location.Organization);
            Assert.Equal(country, location.Country);
            Assert.Equal(region, location.Region);
            Assert.Equal(postal, location.Postal);
            Assert.Equal(latitude, location.Latitude);
            Assert.Equal(longitude, location.Longitude);
        }

        [Theory]
        [InlineData("")]
        [InlineData("256.0.0.0")]
        [InlineData("1.2.3.4.5.6")]
        public void FindLocationByIp_BadValue_ThrowsExpected(string ip)
        {
            // Assert
            Assert.Throws<FormatException>(() => { _searchEngineService.FindLocationByIp(ip); });
        }

        [Theory]
        [InlineData("cit_Akyx", "org_I Yhaxudoma", "cou_YZU", "reg_A", "pos_2819", -22.7796f, 42.9391f)]
        [InlineData("cit_Ypafo", "org_Ewafexilo U Akibef", "cou_EJ", "reg_E", "pos_0257", -153.9313f, 88.382f)]
        [InlineData("cit_Akycogidoly", "org_Omabykygamizewi Uxowyri", "cou_EG", "reg_Akaqiv", "pos_9352", -161.2054f, -152.906f)]
        [InlineData("cit_Ebi ", "org_E Ififip", "cou_AF", "reg_A", "pos_2020798", 43.8805f, 149.8387f)]
        [InlineData("cit_Yxotys V", "org_Obis ", "cou_AB", "reg_Ifeb ", "pos_367695", -53.6623f, -51.4215f)]
        [InlineData("cit_Yhuq", "org_Umacem Ve", "cou_YNO", "reg_Eb", "pos_991751", 63.7658f, 130.1045f)]
        [InlineData("cit_Uvahag Selyzemele", "org_A A Arewekehosa", "cou_ECI", "reg_Yzyb", "pos_4832929", -61.6677f, 43.4612f)]
        [InlineData("cit_Ypa Edip Bib", "org_Ep Halepo Ovawiru Ab", "cou_UJO", "reg_Ug", "pos_1257", -124.1657f, 110.0462f)]
        [InlineData("cit_Asapymymor", "org_Oburojoj ", "cou_ER", "reg_Uzuhy", "pos_696594", -155.2008f, 61.7182f)]
        [InlineData("cit_Efuc Xi Eg M Mo", "org_Awijak Xe Yjoqujozeg", "cou_YH", "reg_Uvub", "pos_30384", -122.8844f, -13.0348f)]
        [InlineData("cit_Okojen Pojebesu", "org_Eky Eg Ja Ijiz ", "cou_ED", "reg_O ", "pos_7985", 87.7945f, -8.0378f)]
        [InlineData("cit_El Tyrolop Tomuh ", "org_E Ul Feheherelif", "cou_AF", "reg_Y", "pos_83496", 71.2344f, 63.0575f)]
        [InlineData("cit_Ivyqusaz Feb", "org_Al Gy ", "cou_OZ", "reg_Ig", "pos_160847", -143.6173f, 168.308f)]
        [InlineData("cit_Edibyn Jydu", "org_Iso", "cou_YQE", "reg_Iqyc X", "pos_33711", -141.4344f, -59.8148f)]
        [InlineData("cit_Ytogekejeteb", "org_Ihofyzypak Ceq", "cou_YKA", "reg_Edolag", "pos_4365", 169.8147f, -27.5106f)]
        [InlineData("cit_Akibi Uvifyza", "org_Yxa Itehal", "cou_EDE", "reg_Ofara", "pos_20709", 117.016f, 159.0093f)]
        [InlineData("cit_Yzu Owoj Z", "org_Epavudix T Syvipedefy If", "cou_UJO", "reg_Oc Ci ", "pos_24305", 47.3722f, 133.5105f)]
        [InlineData("cit_I E O", "org_Ebeqavabe Asemerit Fira", "cou_ERE", "reg_A", "pos_41368", -98.7119f, -70.8205f)]
        [InlineData("cit_Ypijoryp", "org_Avof Sory Asamytite", "cou_IM", "reg_Iqynu", "pos_5630204", 100.691f, -168.1084f)]
        [InlineData("cit_Ov Ka", "org_Ywe", "cou_AGU", "reg_Otex", "pos_5897", -172.9812f, -79.4105f)]
        [InlineData("cit_Yf Pajofaxacyx", "org_Obivylova A Ora", "cou_OPA", "reg_Uty", "pos_1198842", -145.2649f, 62.932f)]
        [InlineData("cit_Y Inohorejebij V", "org_Yr Jat", "cou_EJ", "reg_Ywajag", "pos_5355", 95.4318f, 50.5943f)]
        [InlineData("cit_Avusoxiwa", "org_Obe", "cou_IS", "reg_Ov", "pos_9535696", -31.3224f, 138.648f)]
        [InlineData("cit_Ahyq", "org_Ypazem", "cou_OZY", "reg_Of Xuz", "pos_6288", -147.4524f, -136.1736f)]
        [InlineData("cit_Ypib Taje ", "org_Urepeqysopejebege", "cou_UXU", "reg_Ofat", "pos_54155", -88.1194f, -48.5807f)]
        [InlineData("cit_U Oxi Ef Ly", "org_Idi Ono Ebo", "cou_UM", "reg_Ape O", "pos_5644208", 105.9077f, -107.0158f)]
        [InlineData("cit_Am", "org_Aliweho Ovyc Cis", "cou_UWO", "reg_Ofanit", "pos_730427", 22.2743f, 71.5619f)]
        [InlineData("cit_Yxu Edeva O Ede", "org_Y Olycecygotebokivimadij", "cou_ECI", "reg_Ikex", "pos_27385", -137.0419f, -114.8293f)]
        [InlineData("cit_Enehitij", "org_Ifuqy Ox Gydefef ", "cou_AK", "reg_Uharav", "pos_0410025", 59.4065f, 98.1167f)]
        [InlineData("cit_Azatarofyriw", "org_Yfox Lugacopowuhuni Ubiros", "cou_YZU", "reg_Onyro", "pos_745342", -40.6437f, -12.5499f)]
        [InlineData("cit_Otoqy", "org_Obyxylasakiqusediwyjezer", "cou_OZY", "reg_Eqatic", "pos_480759", -124.3576f, -140.3506f)]
        [InlineData("cit_Ar Lujaz", "org_Oxu Omubu E Uja", "cou_AJ", "reg_Iwi", "pos_8933387", 154.1595f, -75.9228f)]
        [InlineData("cit_Up", "org_Apake", "cou_AV", "reg_Am", "pos_238255", 65.2634f, -33.248f)]
        [InlineData("cit_Idavy", "org_Ujuryqy Ihutipilaraw", "cou_IFY", "reg_A", "pos_9993", -0.8553f, -53.0781f)]
        [InlineData("cit_Epoh Lymubutefa ", "org_Osuje Oh", "cou_YCU", "reg_Ud", "pos_1223553", -54.8739f, -36.1324f)]
        [InlineData("cit_Y A Aq", "org_Ilezokyj Zedo U Uf Gosudy", "cou_YFO", "reg_Ahelec", "pos_03292", -34.4045f, -129.8112f)]
        [InlineData("cit_Os", "org_E One Isivyvifybufezud", "cou_IN", "reg_Ajyf", "pos_45123", -125.8897f, -27.467f)]
        [InlineData("cit_Yvekyfogu Utek", "org_Uwonus S Dazaco Aw Wumefux", "cou_EJ", "reg_U", "pos_169634", -4.7406f, -103.0773f)]
        [InlineData("cit_O Os", "org_A A Oravas Hedegosin", "cou_EG", "reg_A Osu", "pos_5843", -48.7973f, 132.7865f)]
        [InlineData("cit_Ir", "org_Ysaba Ebazicyfet Gura Osa", "cou_YTU", "reg_I", "pos_7049", -98.8755f, 37.0324f)]
        [InlineData("cit_Yvym Sahiken", "org_E Ylyd ", "cou_YBY", "reg_U", "pos_0474", -131.1181f, 1.4555f)]
        [InlineData("cit_Eh Babedid Ven", "org_Oxym", "cou_ERO", "reg_Ex", "pos_61003", 82.0487f, -67.9309f)]
        [InlineData("cit_U Eqetad", "org_O O Ahu Izur", "cou_OW", "reg_Y Elyg", "pos_305802", -160.0226f, -97.8273f)]
        [InlineData("cit_Asaninelihe Ab", "org_Uqaqabe", "cou_EJY", "reg_Et ", "pos_6980624", -116.4104f, 58.87f)]
        [InlineData("cit_Ucotam", "org_Ytihogoxanebyzy Etabo", "cou_OW", "reg_Ozyli", "pos_4593571", -27.4081f, 90.1362f)]
        [InlineData("cit_Yn", "org_I Azeraq Nyqat", "cou_AGU", "reg_Ec", "pos_9636", 67.1269f, 55.1645f)]
        [InlineData("cit_Oq T", "org_Yhyla At L ", "cou_YXA", "reg_Yzev", "pos_67340", 55.256f, 0.0018f)]
        [InlineData("cit_A Afetehyduby", "org_Ozygodawo", "cou_AV", "reg_Edibi", "pos_2136381", 113.0984f, 13.0784f)]
        [InlineData("cit_Ux Ny Er My", "org_Eh Sy", "cou_YXA", "reg_Amin", "pos_1440155", 165.2065f, 94.3463f)]
        [InlineData("cit_Ec Ge", "org_Eqim Qypulacykivub", "cou_UJ", "reg_Ir", "pos_668951", 146.2797f, 27.2371f)]
        [InlineData("cit_O Eq M", "org_Igi Yqa Olaziroj", "cou_AT", "reg_I", "pos_119608", 77.4456f, 156.3739f)]
        [InlineData("cit_Ileso Udico", "org_Idefexagum", "cou_EFY", "reg_Ory", "pos_3608196", -138.8808f, -5.4221f)]
        [InlineData("cit_E Iwobytumewe Awaf", "org_Ehymif", "cou_ORI", "reg_Y Uk P", "pos_25456", -13.8255f, 131.8239f)]
        [InlineData("cit_I ", "org_Apirybemej Gigyxe I", "cou_EJ", "reg_Acam", "pos_6156", 41.9985f, -102.6683f)]
        [InlineData("cit_Ekexop Canov Gy", "org_Yhegutopoh", "cou_AF", "reg_In", "pos_7426", -51.9738f, -92.1037f)]
        [InlineData("cit_Ugy Oq Zucahejydod", "org_A Ytewa U Ytiryvykapic", "cou_AJ", "reg_Ulin", "pos_0254004", -16.1241f, -176.7817f)]
        [InlineData("cit_Ij", "org_Ipog M Gomydo Oku", "cou_ILA", "reg_Uty", "pos_2277180", -177.4071f, -59.6066f)]
        [InlineData("cit_Eqobuxemypyp", "org_Apesyjugity O Igupur Ja", "cou_IS", "reg_Aqimov", "pos_0530", 27.8604f, 16.0567f)]
        [InlineData("cit_Irusuby", "org_Udow Sitirebyheqe", "cou_YFO", "reg_I", "pos_435357", 147.1954f, -122.5615f)]
        [InlineData("cit_Yvegelugojihykeca", "org_Icarynif Laxizet ", "cou_AV", "reg_Ypol", "pos_74220", -2.2773f, 15.9787f)]
        [InlineData("cit_Axojodoji ", "org_Epebabojiz", "cou_IZ", "reg_Yve", "pos_2561832", -152.899f, -124.314f)]
        [InlineData("cit_Obub", "org_Ulo Ivypegah Gam", "cou_IL", "reg_O ", "pos_5382", -77.9092f, 81.1826f)]
        [InlineData("cit_Edogecasase O", "org_Omymicorajof", "cou_IT", "reg_Ewe", "pos_0249974", 81.5759f, 81.6668f)]
        [InlineData("cit_Axaxa E Ule Ojywel", "org_Iqiq Risub Boriqa", "cou_YKY", "reg_Y", "pos_4619021", 164.8978f, -62.6438f)]
        [InlineData("cit_Ecinugib F", "org_Uvo", "cou_OZ", "reg_Ebefuz", "pos_416344", 165.8482f, -61.4708f)]
        [InlineData("cit_Oq", "org_Ekokepi O Abafogo Abuceja", "cou_YH", "reg_Ot Hy", "pos_4062", -160.2648f, -70.2022f)]
        [InlineData("cit_Ec Zy Equj", "org_Amugeb Le", "cou_IKU", "reg_Uva ", "pos_875926", -55.781f, 72.3736f)]
        [InlineData("cit_Yb Buh", "org_Ap Kejak Vu", "cou_ORI", "reg_O Or", "pos_785948", 129.976f, 178.9972f)]
        [InlineData("cit_Yroh", "org_Ipecakexidy", "cou_YKA", "reg_Uvupos", "pos_214688", -115.8856f, -163.955f)]
        [InlineData("cit_Ojifuxerirucyjaf", "org_Ewelu Ajyfirilag Zici", "cou_YS", "reg_A Y", "pos_5832", -15.524f, 115.8385f)]
        [InlineData("cit_Yle", "org_Yvaleq", "cou_YFO", "reg_Ob", "pos_9706", -109.8679f, -76.3609f)]
        [InlineData("cit_Ozy Yjofe Uhoxas", "org_Ovi Ifyhaqilija I", "cou_IB", "reg_Ifohu", "pos_7335536", -38.905f, -164.3822f)]
        [InlineData("cit_Adoveso", "org_Yze A Agupen Dibukeryw", "cou_IZ", "reg_Yqu ", "pos_723410", -159.4408f, -170.1191f)]
        [InlineData("cit_I ", "org_Ozosybeb", "cou_OX", "reg_Yqu ", "pos_7912592", 32.1611f, -172.2823f)]
        [InlineData("cit_Uw Mazapov Ku Uwan", "org_Uxixywosi Y", "cou_UJO", "reg_Up", "pos_699901", 11.2934f, 26.1544f)]
        [InlineData("cit_Omeki", "org_Ovyz Re I Unoc", "cou_OM", "reg_Y ", "pos_882966", -132.8327f, 80.5904f)]
        [InlineData("cit_O ", "org_Ojo Iwezyqy", "cou_OW", "reg_Y", "pos_543759", -3.4209f, -72.9226f)]
        [InlineData("cit_Inalo Orenymum", "org_Ugyqydav Ryd Kit", "cou_YNO", "reg_Iju ", "pos_9224496", -41.697f, 71.3895f)]
        [InlineData("cit_Ebeg", "org_I Iz", "cou_OH", "reg_Ah Quz", "pos_1083257", 67.5341f, 23.1722f)]
        [InlineData("cit_Ityxaleji", "org_Iwywuhacabi Yxec Cah", "cou_UM", "reg_Yjuhyh", "pos_3975", -168.3891f, -6.9746f)]
        [InlineData("cit_Or", "org_Aj Lebun Tegulipib Wic", "cou_YKY", "reg_Ol", "pos_494372", -107.5662f, -11.8088f)]
        [InlineData("cit_Umeby I E", "org_Op Hybomiqez", "cou_YCU", "reg_Or", "pos_6685466", -140.5686f, -45.2281f)]
        [InlineData("cit_Yjucaxil ", "org_Omur Ra Abixat", "cou_UB", "reg_Usove", "pos_862370", 121.0963f, -138.9992f)]
        [InlineData("cit_Iw", "org_Yxubalyc Deh", "cou_USA", "reg_Iwut", "pos_403382", -59.0955f, -64.7005f)]
        [InlineData("cit_Ulyzarobo Ycet", "org_Odydebyzadakejab Cegocad", "cou_IME", "reg_Ovi", "pos_01744", 82.1268f, -95.1874f)]
        [InlineData("cit_Ihisyl M", "org_Upupodapyv ", "cou_OVO", "reg_A Ole", "pos_828939", -142.5576f, 50.4841f)]
        [InlineData("cit_Ete Odabyfoga", "org_Otamajovur Dasarebic", "cou_IFY", "reg_E", "pos_91930", -100.2131f, -69.0439f)]
        [InlineData("cit_Ytykuro Ykehesyxo", "org_Ubigexunyhe E Orogu", "cou_YP", "reg_Uj", "pos_18274", -86.1999f, 109.8789f)]
        [InlineData("cit_Adyvigunyhog", "org_Ew Vy Upuwaponoga", "cou_EG", "reg_Imec", "pos_658070", 152.1438f, 32.8215f)]
        [InlineData("cit_Unog Qiq ", "org_Oguvyveq ", "cou_YCU", "reg_Ofara", "pos_1914", 100.1476f, -34.675f)]
        [InlineData("cit_Ubyqipicaca", "org_Ovegeqeqepinety Abo Ezako", "cou_ERO", "reg_Ajeb ", "pos_7123", -156.1413f, 85.858f)]
        [InlineData("cit_Epahizahy", "org_Ehizybusyj Ve A Evel Job X", "cou_ED", "reg_U", "pos_4016306", -173.0696f, 46.7699f)]
        [InlineData("cit_O A Ymiqil", "org_Enic", "cou_YKA", "reg_Yzi", "pos_7799614", 99.2766f, -93.327f)]
        [InlineData("cit_Ijefu Isacivy Ido", "org_Yjopicuqahop N", "cou_EJ", "reg_Ocinu", "pos_987796", -92.3892f, 69.7896f)]
        [InlineData("cit_Oxix ", "org_Uciheh Zaqateruvisapi", "cou_ERO", "reg_Uve", "pos_5079785", -96.0069f, -74.9317f)]
        [InlineData("cit_Evyhuvep Gu", "org_Yjery O Efoninatir", "cou_YQE", "reg_U", "pos_776559", 103.4733f, -115.4478f)]
        [InlineData("cit_Inym H Kiberijyj", "org_Eq Gexaq L", "cou_ADO", "reg_Age Er", "pos_97364", -168.5181f, 78.9864f)]
        [InlineData("cit_Ofod", "org_Yxynasewysev Hec Ry", "cou_ERE", "reg_Omeka", "pos_4400", -68.9233f, 162.2549f)]
        [InlineData("cit_Okawefoviguwi", "org_An Pypuw Jylof", "cou_URO", "reg_Ak ", "pos_707064", -40.9126f, -170.751f)]
        [InlineData("cit_Omebuporasuger", "org_Ejoriwagoraxeny Or Gytadi ", "cou_ELA", "reg_Igin", "pos_9640344", -160.5897f, 77.0572f)]
        public void FindLocationsByCity_OrdinalSearch_ReturnsExpected(
            string city, 
            string org,
            string country,
            string region,
            string postal,
            float latitude,
            float longitude)
        {
            // Arrange

            // Act
            var locations = _searchEngineService.FindLocationsByCity(city);

            // Assert
            Assert.All(locations, location => location.City.Equals(city));
            Assert.Contains(locations, location =>           
                location.City.Equals(city) &&
                location.Organization.Equals(org) &&
                location.Region.Equals(region) &&
                location.Postal.Equals(postal) &&
                location.Latitude.Equals(latitude) &&
                location.Longitude.Equals(longitude) &&
                location.Country.Equals(country)
            );
        }

        [Fact]
        public void FindLocationsByCity_CheckResultsCount_ReturnsExpected()
        {
            // Arrange
            const string city = "cit_A ";
            const int count = 160;

            // Act
            var locations = _searchEngineService.FindLocationsByCity(city);

            // Assert
            Assert.All(locations, location => location.City.Equals(city));
            Assert.Equal(count, locations.Count());
        }
    }
}
