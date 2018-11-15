using Newtonsoft.Json;

using System;
using System.IO;
using System.Net;
using System.Text;


namespace FreeInfantryClient.Windows.Account.Protocol
{
    public class AccountServer
    {
        /// <summary>
        /// Returns a reason if the server sent us one
        /// </summary>
        public static string Reason { get; private set; }

        /// <summary>
        /// Sends a ping request to our account server
        /// </summary>
        public static Status.PingRequestStatusCode PingAccount(string pingUrl)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(pingUrl);
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";
            try
            {
                using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
                {
                    streamReader.ReadToEnd();
                    return Status.PingRequestStatusCode.Ok;
                }
            }
            catch (WebException ex)
            {
                if ((HttpWebResponse)ex.Response == null)
                { return Status.PingRequestStatusCode.NotFound; }

                switch (((HttpWebResponse)ex.Response).StatusCode)
                {
                    case HttpStatusCode.NotFound:
                    default:
                        return Status.PingRequestStatusCode.NotFound;
                }
            }
        }

        /// <summary>
        /// Sends a register request to our account server
        /// </summary>
        public static Status.RegistrationStatusCode RegisterAccount(Status.RegisterRequestObject requestModel, string RegisterUrl)
        {
            if (requestModel == null)
            { throw new ArgumentNullException("requestModel"); }

            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestModel));
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(RegisterUrl);
            httpWebRequest.Method = "PUT";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = bytes.Length;
            try
            {
                httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
            }
            catch (WebException)
            {
                return Status.RegistrationStatusCode.ServerError;
            }

            try
            {
                using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
                {
                    streamReader.ReadToEnd();
                    return Status.RegistrationStatusCode.Ok;
                }
            }
            catch (WebException ex)
            {
                if ((HttpWebResponse)ex.Response == null)
                { return Status.RegistrationStatusCode.NoResponse; }

                switch (((HttpWebResponse)ex.Response).StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        Reason = ((HttpWebResponse)ex.Response).StatusDescription;
                        return Status.RegistrationStatusCode.UsernameTaken;
                    case HttpStatusCode.Conflict:
                        Reason = ((HttpWebResponse)ex.Response).StatusDescription;
                        return Status.RegistrationStatusCode.EmailTaken;
                    case HttpStatusCode.NotAcceptable:
                        Reason = ((HttpWebResponse)ex.Response).StatusDescription;
                        return Status.RegistrationStatusCode.WeakCredentials;
                    case HttpStatusCode.InternalServerError:
                        Reason = ((HttpWebResponse)ex.Response).StatusDescription;
                        return Status.RegistrationStatusCode.ServerError;
                    case HttpStatusCode.Created:
                        return Status.RegistrationStatusCode.Ok;
                    case HttpStatusCode.BadRequest:
                        return Status.RegistrationStatusCode.MalformedData;

                    default:
                        return Status.RegistrationStatusCode.ServerError;
                }
            }
        }

        /// <summary>
        /// Sends a login request to our account server
        /// </summary>
        public static Status.LoginStatusCode LoginAccount(Status.LoginRequestObject requestModel, string LoginUrl, out Status.LoginResponseObject payload)
        {
            if (requestModel == null)
            { throw new ArgumentNullException("requestModel"); }

            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestModel));
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(LoginUrl);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = bytes.Length;
            try
            {
                httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
            }
            catch (WebException)
            {
                payload = null;
                return Status.LoginStatusCode.ServerError;
            }

            try
            {
                using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
                {
                    string str = streamReader.ReadToEnd();
                    payload = JsonConvert.DeserializeObject<Status.LoginResponseObject>(str);
                    if (string.IsNullOrWhiteSpace(payload.TicketId.ToString()) || string.IsNullOrWhiteSpace(payload.Username))
                    {
                        payload = null;
                        Reason = "Incorrect username or password.";
                        return Status.LoginStatusCode.MalformedData;
                    }
                    return Status.LoginStatusCode.Ok;
                }
            }
            catch (WebException ex)
            {
                if ((HttpWebResponse)ex.Response == null)
                {
                    payload = null;
                    return Status.LoginStatusCode.NoResponse;
                }

                switch (((HttpWebResponse)ex.Response).StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        payload = null;
                        Reason = ((HttpWebResponse)ex.Response).StatusDescription;
                        return Status.LoginStatusCode.MalformedData;
                    case HttpStatusCode.NotFound:
                        payload = null;
                        Reason = ((HttpWebResponse)ex.Response).StatusDescription;
                        return Status.LoginStatusCode.InvalidCredentials;

                    default:
                        payload = null;
                        return Status.LoginStatusCode.ServerError;
                }
            }
        }

        /// <summary>
        /// Sends a recovery request to our account server
        /// </summary>
        public static Status.RecoverStatusCode RecoverAccount(Status.RecoverRequestObject requestModel, string RequestUrl, out string payload)
        {
            payload = null;
            if (requestModel == null)
            { throw new ArgumentNullException("requestModel"); }

            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestModel));
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(RequestUrl);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = bytes.Length;
            try
            {
                httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
            }
            catch (WebException)
            { return Status.RecoverStatusCode.ServerError; }

            try
            {
                using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
                {
                    payload = streamReader.ReadToEnd();
                    return Status.RecoverStatusCode.Ok;
                }
            }
            catch (WebException ex)
            {
                if ((HttpWebResponse)ex.Response == null)
                { return Status.RecoverStatusCode.NoResponse; }

                switch (((HttpWebResponse)ex.Response).StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        Reason = ((HttpWebResponse)ex.Response).StatusDescription;
                        return Status.RecoverStatusCode.MalformedData;
                    case HttpStatusCode.NotFound:
                        Reason = ((HttpWebResponse)ex.Response).StatusDescription;
                        return Status.RecoverStatusCode.InvalidCredentials;
                    case HttpStatusCode.InternalServerError:
                        Reason = ((HttpWebResponse)ex.Response).StatusDescription;
                        return Status.RecoverStatusCode.ServerError;

                    default:
                        return Status.RecoverStatusCode.ServerError;
                }
            }
        }

    }
}