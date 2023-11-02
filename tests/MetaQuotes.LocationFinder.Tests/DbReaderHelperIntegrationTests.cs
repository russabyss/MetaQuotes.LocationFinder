using MetaQuotes.LocationFinder.Contracts;
using MetaQuotes.LocationFinder.Core.Helpers;
using Xunit.Abstractions;

namespace MetaQuotes.LocationFinder.Tests
{
    public class DbReaderHelperIntegrationTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public DbReaderHelperIntegrationTests(ITestOutputHelper output)
        {
            _outputHelper = output;
        }

        [Fact]
        public void GetHeader_ReadFileFromDisk_ReturnsExpected()
        {
            // Arrange
            var buffer = File.ReadAllBytes("Data/geobase.dat");

            // Act
            var header = DbReaderHelper.GetHeader(buffer);

            // Assert
            Assert.Equal("Geo.IP", header.Name);
            Assert.Equal<int>(1, header.Version);
            Assert.Equal<int>(100000, header.Records);
            Assert.Equal<ulong>(1487167858, header.Timestamp);
            Assert.Equal<uint>(10800060, header.OffsetCities);
            Assert.Equal<uint>(60, header.OffsetRanges);
            Assert.Equal<uint>(1200060, header.OffsetLocations);
        }        
        
        [Fact]
        public void GetIntervals_ReadFileFromDisk_ReturnsExpected()
        {
            // Arrange
            var buffer = File.ReadAllBytes("Data/geobase.dat");
            var header = DbReaderHelper.GetHeader(buffer);

            // Act
            var intervals = DbReaderHelper.GetIpIntervals(
                buffer
                    .AsSpan()
                    .Slice((int)header.OffsetRanges, header.Records * DbConstants.IpIntervalLength), // Для упрощения - делаем прямой каст uint -> int, т.к. в текущем файле точно все будет ок.
                header.Records);

            // Assert
            Assert.Equal(header.Records, intervals.Length);
            // TODO: проверить, что IpFrom <= IpTo во всех элементах.
        } 
        
        [Fact]
        public void GetLocations_ReadFileFromDisk_ReturnsExpected()
        {
            // Arrange
            var buffer = File.ReadAllBytes("Data/geobase.dat");
            var header = DbReaderHelper.GetHeader(buffer);

            // Act
            var locations = DbReaderHelper.GetLocations(
                buffer
                    .AsSpan()
                    .Slice((int)header.OffsetLocations, header.Records * DbConstants.LocationLength), // Для упрощения - делаем прямой каст uint -> int, т.к. в текущем файле точно все будет ок.
                header.Records);

            // Assert
            Assert.Equal(header.Records, locations.Length);
            // TODO: проверить, что все city начинаются с cit, etc.
        }  
        
        [Fact]
        public void GetIpIndex_ReadFileFromDisk_ReturnsExpected()
        {
            // Arrange
            var buffer = File.ReadAllBytes("Data/geobase.dat");
            var header = DbReaderHelper.GetHeader(buffer);

            // Act
            var ipSearchIndex = DbReaderHelper.GetIpSearchIndex(
                buffer
                    .AsSpan()
                    .Slice((int)header.OffsetRanges, header.Records * DbConstants.IpIntervalLength), // Для упрощения - делаем прямой каст uint -> int, т.к. в текущем файле точно все будет ок.
                header.Records);

            // Assert
            Assert.Equal(header.Records, ipSearchIndex.IpsFrom.Length);
            Assert.Equal(header.Records, ipSearchIndex.IpsTo.Length);
            Assert.Equal(header.Records, ipSearchIndex.LocationIndexes.Length);
            // TODO: проверить, что все IpTo <= IpFrom на одинаковых индексах массива.
        }     
        
        [Fact]
        public void GetCitySearchIndex_ReadFileFromDisk_ReturnsExpected()
        {
            // Arrange
            var buffer = File.ReadAllBytes("Data/geobase.dat");
            var header = DbReaderHelper.GetHeader(buffer);
            var locations = buffer.AsMemory((int)header.OffsetLocations, DbConstants.LocationLength * header.Records);
            var citiesList = buffer.AsSpan((int)header.OffsetCities, header.Records * DbConstants.LocationsListItem);

            // Act
            var citySearchIndex = DbReaderHelper.GetCitySearchIndex(
                locations,
                citiesList,
                header.Records);

            // Assert
            Assert.Equal(header.Records, citySearchIndex.Cities.Length);
            // TODO: проверить, массив Cities - упорядочен.
        }     
        
        [Fact]
        public void CreateSearchIndex_ReadFileFromDisk_ReturnsExpected()
        {
            // Arrange
            

            // Act
            var searchIndex = DbReaderHelper.CreateSearchIndex("Data/geobase.dat");

            // Assert
            Assert.Equal(searchIndex.Header.Records, searchIndex.CitySearchIndex.Cities.Length);
            Assert.Equal(searchIndex.Header.Records, searchIndex.IpSearchIndex.IpsTo.Length);
            Assert.Equal(searchIndex.Header.Records, searchIndex.IpSearchIndex.IpsFrom.Length);
            Assert.Equal(searchIndex.Header.Records, searchIndex.IpSearchIndex.LocationIndexes.Length);
        }

        [Fact/*(Skip = "Отладочный код для сбора данных из файла")*/]
        public void Debug_GetExactData()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            var db = DbReaderHelper.LoadFromFile("Data/geobase.dat");
            
            for(var i=0; i<100; i++)
            {
                var nextValue = rnd.Next(0, db.Header.Records);
                var ipInterval = db.IpIntervals[nextValue];
                var location = db.Locations[ipInterval.LocationIndex];
                
                _outputHelper.WriteLine(string.Create(System.Globalization.CultureInfo.InvariantCulture,
                    $"[InlineData(" +
                    //$"{nextValue}, " +
                    //$"{ipInterval.LocationIndex}, " +
                    $"\"{ipInterval.IpFrom}\", " +
                    //$"\"{ipInterval.IpTo}\",  " +
                    $"\"{location.City}\", " +
                    $"\"{location.Organization}\", " +
                    $"\"{location.Country}\", " +
                    $"\"{location.Region}\", " +
                    $"\"{location.Postal}\", " +
                    $"{location.Latitude}f, " +
                    $"{location.Longitude}f" +
                    $")]"));    
                
                _outputHelper.WriteLine(string.Create(System.Globalization.CultureInfo.InvariantCulture,
                    $"[InlineData(" +
                    //$"{nextValue}, " +
                    //$"{ipInterval.LocationIndex}, " +
                    //$"\"{ipInterval.IpFrom}\", " +
                    $"\"{ipInterval.IpTo}\",  " +
                    $"\"{location.City}\", " +
                    $"\"{location.Organization}\", " +
                    $"\"{location.Country}\", " +
                    $"\"{location.Region}\", " +
                    $"\"{location.Postal}\", " +
                    $"{location.Latitude}f, " +
                    $"{location.Longitude}f" +
                    $")]"));
            }

            /*  100*2 рандомных записей        
                [InlineData("155.228.48.55", "cit_Ibesex Fe", "org_Ubez Gi Ofo ", "cou_OMO", "reg_Aho", "pos_89041", -140.1777f, 18.6284f)]
                [InlineData("32.132.49.55",  "cit_Ibesex Fe", "org_Ubez Gi Ofo ", "cou_OMO", "reg_Aho", "pos_89041", -140.1777f, 18.6284f)]
                [InlineData("220.141.20.205", "cit_Ytuluju U", "org_Ida Uzonaq Ba Owop F", "cou_AJ", "reg_I ", "pos_97163", -155.4074f, -166.7253f)]
                [InlineData("160.191.20.205",  "cit_Ytuluju U", "org_Ida Uzonaq Ba Owop F", "cou_AJ", "reg_I ", "pos_97163", -155.4074f, -166.7253f)]
                [InlineData("212.160.237.224", "cit_Ybasycadabigyw", "org_Erimo", "cou_ORU", "reg_An G", "pos_90508", -33.3477f, -150.6189f)]
                [InlineData("173.245.238.224",  "cit_Ybasycadabigyw", "org_Erimo", "cou_ORU", "reg_An G", "pos_90508", -33.3477f, -150.6189f)]
                [InlineData("192.249.201.82", "cit_Uwaha", "org_U Exefalod Tawa", "cou_AB", "reg_Ukoxam", "pos_2941261", -16.3405f, 14.5992f)]
                [InlineData("99.141.202.82",  "cit_Uwaha", "org_U Exefalod Tawa", "cou_AB", "reg_Ukoxam", "pos_2941261", -16.3405f, 14.5992f)]
                [InlineData("25.213.253.204", "cit_Ibopuvyvew", "org_Ezuhoqe I", "cou_OPA", "reg_Elup", "pos_0175070", -148.0829f, -71.2928f)]
                [InlineData("242.123.254.204",  "cit_Ibopuvyvew", "org_Ezuhoqe I", "cou_OPA", "reg_Elup", "pos_0175070", -148.0829f, -71.2928f)]
                [InlineData("22.237.112.148", "cit_Ixam Do Alokeg", "org_Ol Micime I Awocipujuger", "cou_OX", "reg_Omiwaw", "pos_187555", 74.4023f, 159.2652f)]
                [InlineData("191.34.114.148",  "cit_Ixam Do Alokeg", "org_Ol Micime I Awocipujuger", "cou_OX", "reg_Omiwaw", "pos_187555", 74.4023f, 159.2652f)]
                [InlineData("137.153.208.208", "cit_Ace U Or Pi", "org_Ufeq", "cou_IB", "reg_Y Iza", "pos_95070", -3.0386f, 107.8946f)]
                [InlineData("223.187.209.208",  "cit_Ace U Or Pi", "org_Ufeq", "cou_IB", "reg_Y Iza", "pos_95070", -3.0386f, 107.8946f)]
                [InlineData("148.164.207.177", "cit_I O Op", "org_Anona Etyhitujicez", "cou_UB", "reg_O Afy", "pos_7947", 171.6118f, 157.4118f)]
                [InlineData("148.164.207.177",  "cit_I O Op", "org_Anona Etyhitujicez", "cou_UB", "reg_O Afy", "pos_7947", 171.6118f, 157.4118f)]
                [InlineData("40.91.12.167", "cit_Ylalu U", "org_Ix Hiti Iqe ", "cou_ERO", "reg_Ewe", "pos_805810", 94.4341f, 162.472f)]
                [InlineData("8.86.13.167",  "cit_Ylalu U", "org_Ix Hiti Iqe ", "cou_ERO", "reg_Ewe", "pos_805810", 94.4341f, 162.472f)]
                [InlineData("105.228.91.76", "cit_Idusikafil De Ufaw", "org_Ojikonyqu Opafu Erejat Si", "cou_IS", "reg_Oki", "pos_683339", -109.3408f, 173.8303f)]
                [InlineData("148.10.92.76",  "cit_Idusikafil De Ufaw", "org_Ojikonyqu Opafu Erejat Si", "cou_IS", "reg_Oki", "pos_683339", -109.3408f, 173.8303f)]
                [InlineData("154.50.42.6", "cit_Er Rage Abexajopaz", "org_Udi Yx Bodahoq Ki I Af", "cou_YH", "reg_Ik", "pos_8509", 76.9303f, 97.8391f)]
                [InlineData("143.234.42.6",  "cit_Er Rage Abexajopaz", "org_Udi Yx Bodahoq Ki I Af", "cou_YH", "reg_Ik", "pos_8509", 76.9303f, 97.8391f)]
                [InlineData("1.22.156.184", "cit_Yxanyki Ufafel ", "org_E Ymav ", "cou_YS", "reg_Yw", "pos_2715087", -110.6781f, -84.5955f)]
                [InlineData("239.68.156.184",  "cit_Yxanyki Ufafel ", "org_E Ymav ", "cou_YS", "reg_Yw", "pos_2715087", -110.6781f, -84.5955f)]
                [InlineData("33.134.177.39", "cit_Imoja Y Upuqade", "org_Ysexy Akofa", "cou_YH", "reg_Uwas", "pos_0826", -126.4118f, 16.7542f)]
                [InlineData("12.15.178.39",  "cit_Imoja Y Upuqade", "org_Ysexy Akofa", "cou_YH", "reg_Uwas", "pos_0826", -126.4118f, 16.7542f)]
                [InlineData("174.33.237.211", "cit_I Yfuqahil Hehela", "org_Esiv Kekysibituz", "cou_ER", "reg_Ekuq", "pos_2889098", 27.416f, 116.2554f)]
                [InlineData("26.246.237.211",  "cit_I Yfuqahil Hehela", "org_Esiv Kekysibituz", "cou_ER", "reg_Ekuq", "pos_2889098", 27.416f, 116.2554f)]
                [InlineData("186.179.108.154", "cit_Awamopazy I Eqabab", "org_Etag P Dyqej", "cou_UM", "reg_O", "pos_710454", 89.3967f, 63.3247f)]
                [InlineData("108.25.110.154",  "cit_Awamopazy I Eqabab", "org_Etag P Dyqej", "cou_UM", "reg_O", "pos_710454", 89.3967f, 63.3247f)]
                [InlineData("13.11.203.154", "cit_Ugi Yre Edo", "org_Ypezanimeni Ofaredyzynip", "cou_UM", "reg_Ec", "pos_68225", -155.0473f, -9.9334f)]
                [InlineData("13.11.203.154",  "cit_Ugi Yre Edo", "org_Ypezanimeni Ofaredyzynip", "cou_UM", "reg_Ec", "pos_68225", -155.0473f, -9.9334f)]
                [InlineData("10.89.60.60", "cit_Izu", "org_Ujepawipabezoc", "cou_EG", "reg_Ulaw ", "pos_50085", -88.0481f, -111.4177f)]
                [InlineData("50.49.61.60",  "cit_Izu", "org_Ujepawipabezoc", "cou_EG", "reg_Ulaw ", "pos_50085", -88.0481f, -111.4177f)]
                [InlineData("65.118.186.95", "cit_Iwab Vyqu Eh Pi", "org_O Yxe Obi Oxu", "cou_AF", "reg_Oxa", "pos_3249201", 79.2629f, 7.8646f)]
                [InlineData("162.219.187.95",  "cit_Iwab Vyqu Eh Pi", "org_O Yxe Obi Oxu", "cou_AF", "reg_Oxa", "pos_3249201", 79.2629f, 7.8646f)]
                [InlineData("92.182.98.224", "cit_Anyly", "org_Uhikof Ty", "cou_EV", "reg_A", "pos_4537", 120.2604f, 102.6927f)]
                [InlineData("114.194.98.224",  "cit_Anyly", "org_Uhikof Ty", "cou_EV", "reg_A", "pos_4537", 120.2604f, 102.6927f)]
                [InlineData("96.215.196.99", "cit_O E", "org_I Omaxupexuz Mu U Awaka", "cou_AT", "reg_E", "pos_5497603", -35.3794f, 135.5203f)]
                [InlineData("122.235.196.99",  "cit_O E", "org_I Omaxupexuz Mu U Awaka", "cou_AT", "reg_E", "pos_5497603", -35.3794f, 135.5203f)]
                [InlineData("118.46.32.13", "cit_Ydajaq Wesilas", "org_U Ah Nacekiwykoq ", "cou_OCE", "reg_E", "pos_9853", 28.0494f, 40.2848f)]
                [InlineData("23.226.32.13",  "cit_Ydajaq Wesilas", "org_U Ah Nacekiwykoq ", "cou_OCE", "reg_E", "pos_9853", 28.0494f, 40.2848f)]
                [InlineData("220.31.105.71", "cit_Eseriv Ga A ", "org_O Acopajesu Y O Upypo U", "cou_ER", "reg_Up", "pos_810778", -66.6469f, -114.3238f)]
                [InlineData("83.238.105.71",  "cit_Eseriv Ga A ", "org_O Acopajesu Y O Upypo U", "cou_ER", "reg_Up", "pos_810778", -66.6469f, -114.3238f)]
                [InlineData("246.146.124.225", "cit_Uvel", "org_Oriwabijetivas M Wo", "cou_IME", "reg_Ygod", "pos_5876295", -143.6165f, -99.0545f)]
                [InlineData("144.160.124.225",  "cit_Uvel", "org_Oriwabijetivas M Wo", "cou_IME", "reg_Ygod", "pos_5876295", -143.6165f, -99.0545f)]
                [InlineData("140.76.203.129", "cit_Ebocucusajeg Be O", "org_Yga Ocis Zukomyzyxekube", "cou_AJ", "reg_Y", "pos_853766", -60.6438f, 105.968f)]
                [InlineData("9.123.204.129",  "cit_Ebocucusajeg Be O", "org_Yga Ocis Zukomyzyxekube", "cou_AJ", "reg_Y", "pos_853766", -60.6438f, 105.968f)]
                [InlineData("73.183.215.189", "cit_Ebiramehytoma", "org_I Ypevucapasejyre Emywo", "cou_YFO", "reg_Ah Quz", "pos_9683616", -148.1815f, -167.1815f)]
                [InlineData("84.209.216.189",  "cit_Ebiramehytoma", "org_I Ypevucapasejyre Emywo", "cou_YFO", "reg_Ah Quz", "pos_9683616", -148.1815f, -167.1815f)]
                [InlineData("12.237.14.76", "cit_On Fileruzul Deg", "org_Iv Fu", "cou_ON", "reg_Eg", "pos_06756", 128.9621f, 134.7627f)]
                [InlineData("12.237.14.76",  "cit_On Fileruzul Deg", "org_Iv Fu", "cou_ON", "reg_Eg", "pos_06756", 128.9621f, 134.7627f)]
                [InlineData("149.222.50.18", "cit_Ysabi E Oxo Aduse", "org_Ejetekite Egugotuvefyqa", "cou_YFO", "reg_Og W", "pos_83341", -15.1361f, 64.0951f)]
                [InlineData("149.222.50.18",  "cit_Ysabi E Oxo Aduse", "org_Ejetekite Egugotuvefyqa", "cou_YFO", "reg_Og W", "pos_83341", -15.1361f, 64.0951f)]
                [InlineData("144.109.0.212", "cit_Ypenixu E", "org_Ev Sujuruvyfutizahaxama Yl", "cou_YTY", "reg_I", "pos_374354", -100.6745f, 82.6627f)]
                [InlineData("146.182.0.212",  "cit_Ypenixu E", "org_Ev Sujuruvyfutizahaxama Yl", "cou_YTY", "reg_I", "pos_374354", -100.6745f, 82.6627f)]
                [InlineData("208.14.224.38", "cit_O Ijekipa", "org_Odytyc", "cou_YQE", "reg_A Uz", "pos_4482", 69.3476f, 45.5584f)]
                [InlineData("208.14.224.38",  "cit_O Ijekipa", "org_Odytyc", "cou_YQE", "reg_A Uz", "pos_4482", 69.3476f, 45.5584f)]
                [InlineData("182.77.97.118", "cit_Axavini Iqu", "org_U Yq Mib Kulecy Irucy ", "cou_IZ", "reg_Ir", "pos_64139", 4.813f, -101.1962f)]
                [InlineData("182.77.97.118",  "cit_Axavini Iqu", "org_U Yq Mib Kulecy Irucy ", "cou_IZ", "reg_Ir", "pos_64139", 4.813f, -101.1962f)]
                [InlineData("168.116.218.104", "cit_Ekexop Canov Gy", "org_Olyze", "cou_YJ", "reg_Aqi", "pos_14148", 102.7853f, 135.9418f)]
                [InlineData("168.116.218.104",  "cit_Ekexop Canov Gy", "org_Olyze", "cou_YJ", "reg_Aqi", "pos_14148", 102.7853f, 135.9418f)]
                [InlineData("217.13.202.181", "cit_I Ak", "org_Agejufavu", "cou_YH", "reg_Ajeb ", "pos_6240", 178.8168f, 132.1466f)]
                [InlineData("173.116.203.181",  "cit_I Ak", "org_Agejufavu", "cou_YH", "reg_Ajeb ", "pos_6240", 178.8168f, 132.1466f)]
                [InlineData("217.237.11.171", "cit_Ygidu Anuq F Ja", "org_Ulovepez Narogujadiqo", "cou_EJ", "reg_Ak", "pos_45176", -173.7119f, 105.0811f)]
                [InlineData("192.113.12.171",  "cit_Ygidu Anuq F Ja", "org_Ulovepez Narogujadiqo", "cou_EJ", "reg_Ak", "pos_45176", -173.7119f, 105.0811f)]
                [InlineData("217.3.33.69", "cit_Aw Supo El", "org_Amoba O U", "cou_AB", "reg_Uku", "pos_031175", -129.5474f, 95.2389f)]
                [InlineData("217.3.33.69",  "cit_Aw Supo El", "org_Amoba O U", "cou_AB", "reg_Uku", "pos_031175", -129.5474f, 95.2389f)]
                [InlineData("4.66.171.20", "cit_E Ucatifybotofic", "org_Ej Qep Tetas Bi", "cou_OH", "reg_Eb", "pos_203209", -15.3254f, 131.9913f)]
                [InlineData("135.23.172.20",  "cit_E Ucatifybotofic", "org_Ej Qep Tetas Bi", "cou_OH", "reg_Eb", "pos_203209", -15.3254f, 131.9913f)]
                [InlineData("5.18.193.9", "cit_Izyj Dif", "org_Eze Izisu Ix Xomibive", "cou_IZ", "reg_Ovu Oq", "pos_991165", -97.5786f, 38.7514f)]
                [InlineData("42.142.193.9",  "cit_Izyj Dif", "org_Eze Izisu Ix Xomibive", "cou_IZ", "reg_Ovu Oq", "pos_991165", -97.5786f, 38.7514f)]
                [InlineData("193.106.81.173", "cit_Ex Bonoliwabysihu", "org_O I Etupunana Eri", "cou_IT", "reg_U", "pos_00061", 72.4686f, -39.1075f)]
                [InlineData("224.123.82.173",  "cit_Ex Bonoliwabysihu", "org_O I Etupunana Eri", "cou_IT", "reg_U", "pos_00061", 72.4686f, -39.1075f)]
                [InlineData("221.251.254.230", "cit_Ud", "org_Ewexyzalon ", "cou_IM", "reg_Yve", "pos_8914", -158.937f, -73.7106f)]
                [InlineData("221.251.254.230",  "cit_Ud", "org_Ewexyzalon ", "cou_IM", "reg_Yve", "pos_8914", -158.937f, -73.7106f)]
                [InlineData("140.245.189.98", "cit_Upo", "org_Ep W Wu", "cou_YKY", "reg_Adamo ", "pos_3239965", -127.2262f, -136.1594f)]
                [InlineData("40.83.191.98",  "cit_Upo", "org_Ep W Wu", "cou_YKY", "reg_Adamo ", "pos_3239965", -127.2262f, -136.1594f)]
                [InlineData("52.171.10.173", "cit_Ywu Asy Ono I", "org_Ykeduziziwa Ujex Wate ", "cou_ORO", "reg_Yd Fuk", "pos_05914", -152.2285f, -118.1126f)]
                [InlineData("248.1.11.173",  "cit_Ywu Asy Ono I", "org_Ykeduziziwa Ujex Wate ", "cou_ORO", "reg_Yd Fuk", "pos_05914", -152.2285f, -118.1126f)]
                [InlineData("224.166.128.102", "cit_Ihekata", "org_Yw F Ta", "cou_UB", "reg_U", "pos_338878", 124.6934f, 78.0914f)]
                [InlineData("224.166.128.102",  "cit_Ihekata", "org_Yw F Ta", "cou_UB", "reg_U", "pos_338878", 124.6934f, 78.0914f)]
                [InlineData("50.34.150.29", "cit_Imiqyha", "org_Y Uhi ", "cou_IN", "reg_E", "pos_0991608", 20.1967f, 1.3089f)]
                [InlineData("214.47.151.29",  "cit_Imiqyha", "org_Y Uhi ", "cou_IN", "reg_E", "pos_0991608", 20.1967f, 1.3089f)]
                [InlineData("213.136.1.10", "cit_Esynisuvafap Wob", "org_Abib Titel Japoramefu Yw", "cou_ILA", "reg_Iz", "pos_42950", 149.5399f, -149.7097f)]
                [InlineData("255.150.1.10",  "cit_Esynisuvafap Wob", "org_Abib Titel Japoramefu Yw", "cou_ILA", "reg_Iz", "pos_42950", 149.5399f, -149.7097f)]
                [InlineData("21.220.166.193", "cit_O A Ymyrymo", "org_Ow Q Ham", "cou_AF", "reg_O Evu", "pos_99294", -172.1434f, -168.3957f)]
                [InlineData("84.227.167.193",  "cit_O A Ymyrymo", "org_Ow Q Ham", "cou_AF", "reg_O Evu", "pos_99294", -172.1434f, -168.3957f)]
                [InlineData("248.106.99.180", "cit_U Uky Axefeqepobax", "org_Anujevedow Darabuv", "cou_OVO", "reg_Utite", "pos_5382556", 16.1103f, -162.2211f)]
                [InlineData("227.10.100.180",  "cit_U Uky Axefeqepobax", "org_Anujevedow Darabuv", "cou_OVO", "reg_Utite", "pos_5382556", 16.1103f, -162.2211f)]
                [InlineData("216.203.207.43", "cit_Ofyhitux", "org_Yzecylybomy", "cou_EDE", "reg_Iw F", "pos_44403", -11.5803f, 9.0939f)]
                [InlineData("190.225.208.43",  "cit_Ofyhitux", "org_Yzecylybomy", "cou_EDE", "reg_Iw F", "pos_44403", -11.5803f, 9.0939f)]
                [InlineData("188.69.33.89", "cit_Ih Zas Matyf", "org_E Yratoc Xah", "cou_IKU", "reg_Isynu", "pos_96813", -56.9786f, 154.1182f)]
                [InlineData("210.111.33.89",  "cit_Ih Zas Matyf", "org_E Yratoc Xah", "cou_IKU", "reg_Isynu", "pos_96813", -56.9786f, 154.1182f)]
                [InlineData("37.216.164.34", "cit_Azo Yvavofenur", "org_Yt Me Em Q Ta", "cou_IME", "reg_Ec", "pos_611059", 70.5542f, -146.6111f)]
                [InlineData("179.251.164.34",  "cit_Azo Yvavofenur", "org_Yt Me Em Q Ta", "cou_IME", "reg_Ec", "pos_611059", 70.5542f, -146.6111f)]
                [InlineData("61.201.149.26", "cit_Ibibu", "org_Ig Fejyjowo E Ibubes ", "cou_AF", "reg_Onop", "pos_36378", -152.7541f, 173.7903f)]
                [InlineData("192.221.149.26",  "cit_Ibibu", "org_Ig Fejyjowo E Ibubes ", "cou_AF", "reg_Onop", "pos_36378", -152.7541f, 173.7903f)]
                [InlineData("23.178.0.27", "cit_Eqil", "org_Olexage Oqu Eti Uqutyx", "cou_IB", "reg_Y Aze", "pos_4700", -142.3704f, 27.5033f)]
                [InlineData("102.165.1.27",  "cit_Eqil", "org_Olexage Oqu Eti Uqutyx", "cou_IB", "reg_Y Aze", "pos_4700", -142.3704f, 27.5033f)]
                [InlineData("158.220.4.105", "cit_Aqohe", "org_Iniky", "cou_YCU", "reg_O ", "pos_91432", -30.142f, -97.3232f)]
                [InlineData("158.220.4.105",  "cit_Aqohe", "org_Iniky", "cou_YCU", "reg_O ", "pos_91432", -30.142f, -97.3232f)]
                [InlineData("27.209.240.2", "cit_Ydajaq Wesilas", "org_Ovy Eqo Uso Okasyke U", "cou_AV", "reg_Ec", "pos_6936209", 163.6679f, -153.5108f)]
                [InlineData("27.209.240.2",  "cit_Ydajaq Wesilas", "org_Ovy Eqo Uso Okasyke U", "cou_AV", "reg_Ec", "pos_6936209", 163.6679f, -153.5108f)]
                [InlineData("115.161.211.105", "cit_Esu Arixazanehe", "org_Ilevimuw Fada Elu Iwavoti", "cou_IKU", "reg_A", "pos_205082", -75.0172f, -168.4767f)]
                [InlineData("115.161.211.105",  "cit_Esu Arixazanehe", "org_Ilevimuw Fada Elu Iwavoti", "cou_IKU", "reg_A", "pos_205082", -75.0172f, -168.4767f)]
                [InlineData("221.8.161.119", "cit_Ynac G Qeto Ober W", "org_Ytexesifob", "cou_OQA", "reg_Uzyb", "pos_9050148", 91.0724f, -20.4397f)]
                [InlineData("60.25.161.119",  "cit_Ynac G Qeto Ober W", "org_Ytexesifob", "cou_OQA", "reg_Uzyb", "pos_9050148", 91.0724f, -20.4397f)]
                [InlineData("69.170.46.117", "cit_Ijejuba ", "org_Epuvelapemedep Naj", "cou_UCY", "reg_Y", "pos_4079", -152.1083f, 100.3929f)]
                [InlineData("125.41.47.117",  "cit_Ijejuba ", "org_Epuvelapemedep Naj", "cou_UCY", "reg_Y", "pos_4079", -152.1083f, 100.3929f)]
                [InlineData("99.210.23.215", "cit_Adur Tozexo Ic ", "org_Ek G Voc Cyd", "cou_OMO", "reg_A", "pos_6709", 142.3073f, -74.3337f)]
                [InlineData("6.26.25.215",  "cit_Adur Tozexo Ic ", "org_Ek G Voc Cyd", "cou_OMO", "reg_A", "pos_6709", 142.3073f, -74.3337f)]
                [InlineData("109.62.81.211", "cit_Alosagyg L", "org_Axopewu Yh", "cou_YSY", "reg_Uva", "pos_16786", 55.4198f, -68.08f)]
                [InlineData("191.136.82.211",  "cit_Alosagyg L", "org_Axopewu Yh", "cou_YSY", "reg_Uva", "pos_16786", 55.4198f, -68.08f)]
                [InlineData("177.57.146.94", "cit_Apakare Ocytu I", "org_Al W Modab Fesi O ", "cou_YFO", "reg_Exoc", "pos_640680", -43.3109f, 155.2488f)]
                [InlineData("18.142.146.94",  "cit_Apakare Ocytu I", "org_Al W Modab Fesi O ", "cou_YFO", "reg_Exoc", "pos_640680", -43.3109f, 155.2488f)]
                [InlineData("194.166.78.124", "cit_Uribykoba Ogeluli", "org_Izu Ifo Udexen", "cou_EFA", "reg_Ug", "pos_304038", -63.8089f, 85.747f)]
                [InlineData("224.254.79.124",  "cit_Uribykoba Ogeluli", "org_Izu Ifo Udexen", "cou_EFA", "reg_Ug", "pos_304038", -63.8089f, 85.747f)]
                [InlineData("138.118.171.128", "cit_Acewyvehisehywyb", "org_Exum Qoda Elequm Vihi ", "cou_EJ", "reg_Ocu", "pos_1233", 150.9928f, -123.0898f)]
                [InlineData("237.114.172.128",  "cit_Acewyvehisehywyb", "org_Exum Qoda Elequm Vihi ", "cou_EJ", "reg_Ocu", "pos_1233", 150.9928f, -123.0898f)]
                [InlineData("209.211.93.130", "cit_Uced M Fi", "org_Uda ", "cou_YWU", "reg_In", "pos_86707", -47.2984f, 50.6084f)]
                [InlineData("235.8.95.130",  "cit_Uced M Fi", "org_Uda ", "cou_YWU", "reg_In", "pos_86707", -47.2984f, 50.6084f)]
                [InlineData("104.1.156.228", "cit_O Adofobyvitipyk", "org_A Oluqodyfev Reloqoxic", "cou_AF", "reg_Uhyte", "pos_5874", -91.1478f, -88.0092f)]
                [InlineData("79.78.157.228",  "cit_O Adofobyvitipyk", "org_A Oluqodyfev Reloqoxic", "cou_AF", "reg_Uhyte", "pos_5874", -91.1478f, -88.0092f)]
                [InlineData("60.31.60.139", "cit_Eqil", "org_Eteky", "cou_YKE", "reg_Ecuto ", "pos_3104719", 12.3763f, -98.219f)]
                [InlineData("60.31.60.139",  "cit_Eqil", "org_Eteky", "cou_YKE", "reg_Ecuto ", "pos_3104719", 12.3763f, -98.219f)]
                [InlineData("3.20.187.48", "cit_Ujocusovaz", "org_Or Qy", "cou_YNO", "reg_Uzyg B", "pos_9173617", 56.8769f, 69.646f)]
                [InlineData("3.20.187.48",  "cit_Ujocusovaz", "org_Or Qy", "cou_YNO", "reg_Uzyg B", "pos_9173617", 56.8769f, 69.646f)]
                [InlineData("85.32.0.203", "cit_Iwu", "org_Osapame Oliz Jikujyzojaxuj", "cou_UJO", "reg_Uva", "pos_5380", -85.1786f, 179.5935f)]
                [InlineData("183.82.1.203",  "cit_Iwu", "org_Osapame Oliz Jikujyzojaxuj", "cou_UJO", "reg_Uva", "pos_5380", -85.1786f, 179.5935f)]
                [InlineData("123.132.10.56", "cit_Upixog Ryj Da U", "org_Iko Odat Du Uxes", "cou_OCE", "reg_Y", "pos_413409", 164.9213f, 99.885f)]
                [InlineData("155.229.11.56",  "cit_Upixog Ryj Da U", "org_Iko Odat Du Uxes", "cou_OCE", "reg_Y", "pos_413409", 164.9213f, 99.885f)]
                [InlineData("209.252.140.13", "cit_Enebevenojabim Ja ", "org_Oqe Iwupenymek Juboduvyka", "cou_ER", "reg_Us", "pos_34788", -26.4036f, -122.8172f)]
                [InlineData("173.194.141.13",  "cit_Enebevenojabim Ja ", "org_Oqe Iwupenymek Juboduvyka", "cou_ER", "reg_Us", "pos_34788", -26.4036f, -122.8172f)]
                [InlineData("142.156.5.202", "cit_Yl Ruxaxok", "org_Erytepibot Mora", "cou_UJ", "reg_Yx", "pos_417324", -133.2863f, -168.4492f)]
                [InlineData("18.31.6.202",  "cit_Yl Ruxaxok", "org_Erytepibot Mora", "cou_UJ", "reg_Yx", "pos_417324", -133.2863f, -168.4492f)]
                [InlineData("202.222.225.190", "cit_Ofa", "org_Iwo", "cou_YS", "reg_Yki", "pos_948667", -150.3519f, 90.805f)]
                [InlineData("31.11.227.190",  "cit_Ofa", "org_Iwo", "cou_YS", "reg_Yki", "pos_948667", -150.3519f, 90.805f)]
                [InlineData("203.130.194.144", "cit_Is Zeq Jojez", "org_Ebucaka Iluhowixagavy", "cou_IN", "reg_Oryk", "pos_43924", -93.9086f, -156.6382f)]
                [InlineData("52.168.195.144",  "cit_Is Zeq Jojez", "org_Ebucaka Iluhowixagavy", "cou_IN", "reg_Oryk", "pos_43924", -93.9086f, -156.6382f)]
                [InlineData("178.136.52.213", "cit_Ykeqi", "org_Atitotowagaq", "cou_ERE", "reg_Owoba", "pos_100098", -135.2956f, -47.3919f)]
                [InlineData("6.230.52.213",  "cit_Ykeqi", "org_Atitotowagaq", "cou_ERE", "reg_Owoba", "pos_100098", -135.2956f, -47.3919f)]
                [InlineData("206.25.246.60", "cit_Yg Sydedubabu U Ov", "org_Aducigeq", "cou_OZ", "reg_Eke", "pos_7254", 36.206f, -120.4259f)]
                [InlineData("23.64.246.60",  "cit_Yg Sydedubabu U Ov", "org_Aducigeq", "cou_OZ", "reg_Eke", "pos_7254", 36.206f, -120.4259f)]
                [InlineData("83.210.181.228", "cit_Ilezypyvifileqiguc", "org_Ila", "cou_YXA", "reg_Op", "pos_5663", -63.6177f, 65.9666f)]
                [InlineData("158.37.183.228",  "cit_Ilezypyvifileqiguc", "org_Ila", "cou_YXA", "reg_Op", "pos_5663", -63.6177f, 65.9666f)]
                [InlineData("30.122.23.51", "cit_Ilege Uvad Le", "org_Oqylequxikirohacel", "cou_USA", "reg_U", "pos_6216", 25.4663f, 74.0594f)]
                [InlineData("67.3.24.51",  "cit_Ilege Uvad Le", "org_Oqylequxikirohacel", "cou_USA", "reg_U", "pos_6216", 25.4663f, 74.0594f)]
                [InlineData("64.8.255.228", "cit_Yqigasiga Yvalaju", "org_I Az ", "cou_YCU", "reg_Alimos", "pos_27181", -91.7403f, 68.573f)]
                [InlineData("189.202.255.228",  "cit_Yqigasiga Yvalaju", "org_I Az ", "cou_YCU", "reg_Alimos", "pos_27181", -91.7403f, 68.573f)]
                [InlineData("109.58.33.215", "cit_Ime", "org_Ajiziqofuven Tatawyr", "cou_YJ", "reg_Eqeci", "pos_8890665", -168.5242f, 39.0412f)]
                [InlineData("109.58.33.215",  "cit_Ime", "org_Ajiziqofuven Tatawyr", "cou_YJ", "reg_Eqeci", "pos_8890665", -168.5242f, 39.0412f)]
                [InlineData("141.89.181.61", "cit_Yr Xaha Ufeda", "org_Enahakevo", "cou_UM", "reg_In", "pos_4656417", 153.6918f, -56.0769f)]
                [InlineData("69.157.181.61",  "cit_Yr Xaha Ufeda", "org_Enahakevo", "cou_UM", "reg_In", "pos_4656417", 153.6918f, -56.0769f)]
                [InlineData("186.186.172.31", "cit_Ezare Apit D", "org_Edysenakameqage", "cou_AGU", "reg_U", "pos_5103982", -33.9137f, -86.101f)]
                [InlineData("53.59.173.31",  "cit_Ezare Apit D", "org_Edysenakameqage", "cou_AGU", "reg_U", "pos_5103982", -33.9137f, -86.101f)]
                [InlineData("183.50.183.180", "cit_Ywab Poco", "org_E Oteg ", "cou_AB", "reg_Ebanak", "pos_1574", -137.6735f, -107.1127f)]
                [InlineData("184.0.184.180",  "cit_Ywab Poco", "org_E Oteg ", "cou_AB", "reg_Ebanak", "pos_1574", -137.6735f, -107.1127f)]
                [InlineData("42.33.9.146", "cit_Ekad Jivetapeby", "org_Eduziva Ubedote", "cou_YQE", "reg_Yn J", "pos_20395", -4.5692f, -8.2302f)]
                [InlineData("129.69.10.146",  "cit_Ekad Jivetapeby", "org_Eduziva Ubedote", "cou_YQE", "reg_Yn J", "pos_20395", -4.5692f, -8.2302f)]
                [InlineData("28.136.112.218", "cit_Ugyzu Ed", "org_Ytatac ", "cou_ESI", "reg_Y U", "pos_1025708", -38.2363f, 127.8873f)]
                [InlineData("132.156.113.218",  "cit_Ugyzu Ed", "org_Ytatac ", "cou_ESI", "reg_Y U", "pos_1025708", -38.2363f, 127.8873f)]
                [InlineData("253.86.67.215", "cit_Ybewe Ala Omaz", "org_Ybov F F", "cou_IFY", "reg_Aja A", "pos_97457", -109.6189f, 81.3648f)]
                [InlineData("253.86.67.215",  "cit_Ybewe Ala Omaz", "org_Ybov F F", "cou_IFY", "reg_Aja A", "pos_97457", -109.6189f, 81.3648f)]
                [InlineData("31.42.49.185", "cit_Y Idajojezagaberab", "org_Itoro ", "cou_IKU", "reg_Yjuhyh", "pos_2990821", 11.666f, 178.7335f)]
                [InlineData("31.42.49.185",  "cit_Y Idajojezagaberab", "org_Itoro ", "cou_IKU", "reg_Yjuhyh", "pos_2990821", 11.666f, 178.7335f)]
                [InlineData("112.10.220.11", "cit_O Ac N", "org_Esusoqo Obirohew", "cou_IKU", "reg_Omeka", "pos_5744", 157.045f, 163.0887f)]
                [InlineData("112.10.220.11",  "cit_O Ac N", "org_Esusoqo Obirohew", "cou_IKU", "reg_Omeka", "pos_5744", 157.045f, 163.0887f)]
                [InlineData("9.22.166.13", "cit_Ujymudahucabiwoh ", "org_Od Q Qec Zocozamenaxo", "cou_YQE", "reg_Opiva", "pos_5551", -135.3221f, 31.1028f)]
                [InlineData("9.22.166.13",  "cit_Ujymudahucabiwoh ", "org_Od Q Qec Zocozamenaxo", "cou_YQE", "reg_Opiva", "pos_5551", -135.3221f, 31.1028f)]
                [InlineData("66.244.186.101", "cit_Era Ujuq Tysigi O", "org_Orelek Go", "cou_AJ", "reg_Edul", "pos_5584363", -85.7218f, 30.954f)]
                [InlineData("137.77.188.101",  "cit_Era Ujuq Tysigi O", "org_Orelek Go", "cou_AJ", "reg_Edul", "pos_5584363", -85.7218f, 30.954f)]
                [InlineData("113.247.220.75", "cit_Umim Zedefyqy Ucyn", "org_A Exeluryc N Te Ycupu ", "cou_UPA", "reg_Uhupy ", "pos_03048", 9.1275f, -72.8525f)]
                [InlineData("129.87.221.75",  "cit_Umim Zedefyqy Ucyn", "org_A Exeluryc N Te Ycupu ", "cou_UPA", "reg_Uhupy ", "pos_03048", 9.1275f, -72.8525f)]
                [InlineData("67.74.151.51", "cit_Uwagycyguwyn", "org_Izemidaviz Fobuz", "cou_IKU", "reg_Y", "pos_568122", 144.7523f, -37.3088f)]
                [InlineData("170.168.151.51",  "cit_Uwagycyguwyn", "org_Izemidaviz Fobuz", "cou_IKU", "reg_Y", "pos_568122", 144.7523f, -37.3088f)]
                [InlineData("200.68.221.182", "cit_Ar", "org_Oquvoduvek Mal ", "cou_ED", "reg_Ipedi", "pos_6981801", -63.1731f, 80.342f)]
                [InlineData("49.236.221.182",  "cit_Ar", "org_Oquvoduvek Mal ", "cou_ED", "reg_Ipedi", "pos_6981801", -63.1731f, 80.342f)]
                [InlineData("168.196.88.37", "cit_Ojaketukyga ", "org_Oqixonomopegy", "cou_OZ", "reg_Yt Ke ", "pos_74064", -50.5331f, 114.297f)]
                [InlineData("155.234.89.37",  "cit_Ojaketukyga ", "org_Oqixonomopegy", "cou_OZ", "reg_Yt Ke ", "pos_74064", -50.5331f, 114.297f)]
                [InlineData("195.185.17.30", "cit_Ah Cexa", "org_Ixer", "cou_ERE", "reg_Ewe", "pos_6831", 146.6399f, -3.1487f)]
                [InlineData("56.146.18.30",  "cit_Ah Cexa", "org_Ixer", "cou_ERE", "reg_Ewe", "pos_6831", 146.6399f, -3.1487f)]
                [InlineData("187.72.146.78", "cit_Aq Zobu ", "org_A Ydanymag", "cou_UXU", "reg_Ojafab", "pos_34521", 130.1993f, 73.0434f)]
                [InlineData("187.72.146.78",  "cit_Aq Zobu ", "org_A Ydanymag", "cou_UXU", "reg_Ojafab", "pos_34521", 130.1993f, 73.0434f)]
                [InlineData("183.232.25.106", "cit_Azaquv", "org_I Ubesurac Nebyciwo Ixedu", "cou_ERE", "reg_Igin", "pos_263270", -134.3358f, 80.9329f)]
                [InlineData("183.232.25.106",  "cit_Azaquv", "org_I Ubesurac Nebyciwo Ixedu", "cou_ERE", "reg_Igin", "pos_263270", -134.3358f, 80.9329f)]
                [InlineData("187.56.171.201", "cit_Yk", "org_Awoxujepiravid Zelub Ha", "cou_IS", "reg_Oma", "pos_144356", -95.3058f, 166.852f)]
                [InlineData("52.152.172.201",  "cit_Yk", "org_Awoxujepiravid Zelub Ha", "cou_IS", "reg_Oma", "pos_144356", -95.3058f, 166.852f)]
                [InlineData("126.132.218.42", "cit_Yziqefopy ", "org_Emojacada Epat Kazavev", "cou_OVO", "reg_I Ot ", "pos_9575", -44.2895f, -177.3372f)]
                [InlineData("126.132.218.42",  "cit_Yziqefopy ", "org_Emojacada Epat Kazavev", "cou_OVO", "reg_I Ot ", "pos_9575", -44.2895f, -177.3372f)]
                [InlineData("15.78.172.105", "cit_Isadidenudi I", "org_Uxifemowu Ajecigedut", "cou_ED", "reg_Yz", "pos_7561", -137.4881f, -86.4964f)]
                [InlineData("118.188.172.105",  "cit_Isadidenudi I", "org_Uxifemowu Ajecigedut", "cou_ED", "reg_Yz", "pos_7561", -137.4881f, -86.4964f)]
                [InlineData("161.205.115.121", "cit_Ede Edyqa Ad ", "org_Ohat J", "cou_YXA", "reg_Osew", "pos_2951", 101.8944f, -2.2859f)]
                [InlineData("185.254.116.121",  "cit_Ede Edyqa Ad ", "org_Ohat J", "cou_YXA", "reg_Osew", "pos_2951", 101.8944f, -2.2859f)]
                [InlineData("234.76.203.192", "cit_Uloxih Ku Ebelod", "org_Ot Vojas Pozimofusevicu", "cou_EG", "reg_Aka", "pos_9908792", 76.245f, 107.8757f)]
                [InlineData("39.41.204.192",  "cit_Uloxih Ku Ebelod", "org_Ot Vojas Pozimofusevicu", "cou_EG", "reg_Aka", "pos_9908792", 76.245f, 107.8757f)]
                [InlineData("141.108.95.222", "cit_Eqonavi Eki ", "org_Exyt Vavyryg Gesiv", "cou_ER", "reg_Y ", "pos_5189", 35.7496f, 2.7627f)]
                [InlineData("3.125.96.222",  "cit_Eqonavi Eki ", "org_Exyt Vavyryg Gesiv", "cou_ER", "reg_Y ", "pos_5189", 35.7496f, 2.7627f)]
                [InlineData("224.62.161.124", "cit_Ivybe Ocejufyvu", "org_Yvyf Bazac", "cou_OW", "reg_Omeka", "pos_0689697", -78.0823f, -38.8905f)]
                [InlineData("225.89.161.124",  "cit_Ivybe Ocejufyvu", "org_Yvyf Bazac", "cou_OW", "reg_Omeka", "pos_0689697", -78.0823f, -38.8905f)]
            */
        }
    }
}