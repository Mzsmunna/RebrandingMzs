using ModelContextProtocol.Server;
using Mzstruct.Base.Entities;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mzstruct.MCP.Tools
{
    [McpServerToolType]
    public class UserTools(IBaseUserRepository<BaseUser> repo)
    {
        [McpServerTool, Description("Get all users")]
        public async Task<List<BaseUser>> GetAllUsers()
        {    
            return await repo.GetAll();
        }

        [McpServerTool, Description("Get user by ID")]
        public async Task<BaseUser?> GetById([Description("The ID of the user to retrieve")] string id)
        {
            return await repo.GetById(id);
        }

        [McpServerTool, Description("Get user by field value")]
        public async Task<List<BaseUser>> GetByFieldValue([Description("The name of the field to search by, can be any property of the user")] string fieldName, [Description("The value of the field to search for")] string fieldValue)
        {
            return await repo.GetByFieldValue(fieldName, fieldValue);
        }
    }
}
