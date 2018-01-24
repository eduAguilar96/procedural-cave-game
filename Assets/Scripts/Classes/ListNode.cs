using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListNode<T> {

    public T data;
    public ListNode<T> next;

    //public ListNode() {
    //    data = null;
    //    next = null;
    //}

    public ListNode(T data){
        this.data = data;
        next = null;
    }

    //public void Add(T data) {
    //    while () {

    //    }
    //}
}
