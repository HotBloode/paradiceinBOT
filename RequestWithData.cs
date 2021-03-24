using RestSharp;

namespace paradiceinBOT
{
    public class RequestWithData
    {
        private RestClient client = new RestClient("https://api.paradice.in/api.php");
        private RestRequest request = new RestRequest(Method.POST);
        private string token;
       

        private void AddHeaders()
        {
            request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"87\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"87\"");
            request.AddHeader("DNT", "1");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("Authorization", token);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("accept", "*/*");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            request.AddHeader("Sec-Fetch-Site", "same-site");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Cookie", "__cfduid=da2abf5f89fc9f7f31d7bd151f7803ba91611183385");
            request.AddParameter("application/json", "", ParameterType.RequestBody);
        }

        public  RequestWithData(string token)
        {
            this.token = token;

            AddHeaders();
        }

        private void AddBody(string body)
        {
            request.Parameters[10].Value = body;
        }

        public IRestResponse GetData(string body)
        {
            AddBody(body);
            IRestResponse response = client.Execute(request);

            return response;
        }

    }
}