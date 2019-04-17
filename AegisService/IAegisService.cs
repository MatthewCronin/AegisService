using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System;

namespace AegisService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAegisService" in both code and config file together.
    [ServiceContract]
    public interface IAegisService
    {
        //////Users
        ///////////////////////
        [OperationContract]
        [WebInvoke(UriTemplate = "PostUser",
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string PostUser(User u);

        [OperationContract]
        [WebInvoke(UriTemplate = "/ValidateUser/{uName}/{pword}",
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        int ValidateUser(string uName, string pword);
        ///////////////////////
        //////End Users
        //////ToDo
        ///////////////////////
        [OperationContract]
        [WebGet(UriTemplate = "/GetToDo/{ToDoID}",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ToDo GetToDo(string ToDoID);

        [OperationContract]
        [WebGet(UriTemplate = "/GetToDos/{UserID}",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<ToDo> GetToDos(string UserID);

        [OperationContract()]
        [WebInvoke(UriTemplate = "PostToDo",
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        int PostToDo(ToDo todo);

        [OperationContract]
        [WebInvoke(UriTemplate = "PutToDo",
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        int PutToDo(ToDo todo);

        [OperationContract]
        [WebInvoke(UriTemplate = "DisableToDo",
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        int DisableToDo(ToDo todo);

        [OperationContract]
        [WebInvoke(UriTemplate = "CompleteToDo",
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        int CompleteToDo(ToDo todo);
        ///////////////////////
        //////End ToDo

        //////Security Questions
        ///////////////////////
        [OperationContract]
        [WebGet(UriTemplate = "/GetSecQuestions/",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<SecurityQuestions> GetSecQuestions();
        ///////////////////////
        ////// End Security Questions
    }


    [DataContract]
    public class User
    {
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public int SecQuestion1 { get; set; }
        [DataMember]
        public int SecQuestion2 { get; set; }
        [DataMember]
        public string SecAnswer1 { get; set; }
        [DataMember]
        public string SecAnswer2 { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public int DisabledBy { get; set; }
        [DataMember]
        public DateTime DisabledDate { get; set; }
        [DataMember]
        public bool Disabled { get; set; }
    }
    [DataContract]
    public class ToDo
    {
        [DataMember]
        public int? ToDoID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public int? CreatedBy { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedBy { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
        [DataMember]
        public int? DisabledBy { get; set; }
        [DataMember]
        public string DisabledDate { get; set; }
        [DataMember]
        public bool? Disabled { get; set; }
        [DataMember]
        public bool? Completed { get; set; }
    }
    [DataContract(Namespace = "")]
    public class SecurityQuestions
    {
        [DataMember]
        public int SecQuestionID { get; set; }
        [DataMember]
        public string SecQuestion { get; set; }
    }
}
