﻿namespace Bluedit.Entities;

public class Topic
{    
    public string TopicName { get; set; } = string.Empty;
    public string TopicDescription { get; set; }= string.Empty;
    public List<Post> Posts { get; set; }  
}
