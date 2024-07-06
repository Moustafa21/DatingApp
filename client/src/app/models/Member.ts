import { Photo } from "./Photo"

export interface Memeber {
    id: number
    userName: string
    age: number
    photoUrl: string
    knownAs: string
    created: Date
    introduction: string
    interests: string
    lastActive: Date
    gender: string
    country: string
    city: string
    lookingFor: string
    photos: Photo[]
  }
  
  